using Microsoft.ML;
using ProductPrediction.API.AIService.Dtos;
using ProductPrediction.API.AIService.Interfaces;
using ProductPrediction.API.Entities;

namespace ProductPrediction.API.Services;

public class PurchasePredictionService : IPurchasePredictionService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public PurchasePredictionService(IWebHostEnvironment env)
    {
        _mlContext = new MLContext();

        var modelPath = Path.Combine(env.ContentRootPath, "model.zip");

        if (!File.Exists(modelPath))
            throw new FileNotFoundException($"model.zip não encontrado em {modelPath}");

        _model = _mlContext.Model.Load(modelPath, out _);
    }

    public PurchasePredictionResponse PredictSingle(PurchasePredictionRequest request)
    {
        var engine = _mlContext.Model.CreatePredictionEngine<PurchaseData, PurchasePrediction>(_model);

        var input = new PurchaseData
        {
            UserId = request.UserId.ToString(),
            ItemId = request.Item,
            LastIntervalDays = request.LastIntervalDays,
            AverageIntervalUserItem = request.AverageIntervalUserItem,
            PurchaseDayOfWeek = request.PurchaseDayOfWeek,
            DaysUntilNextPurchase = 0
        };

        var pred = engine.Predict(input);

        return new PurchasePredictionResponse
        {
            DaysUntilNextPurchase = pred.Score,
            PredictedDate = request.LastPurchaseDate?.AddDays(pred.Score)
        };
    }

    public BatchPurchasePredictionResponse PredictBatch(BatchPurchasePredictionRequest request)
    {
        var response = new BatchPurchasePredictionResponse { UserId = request.UserId };

        var engine = _mlContext.Model.CreatePredictionEngine<PurchaseData, PurchasePrediction>(_model);

        foreach (var item in request.Items)
        {
            var input = new PurchaseData
            {
                UserId = request.UserId.ToString(),
                ItemId = item.Item,
                LastIntervalDays = item.LastIntervalDays,
                AverageIntervalUserItem = item.AverageIntervalUserItem,
                PurchaseDayOfWeek = item.PurchaseDayOfWeek
            };

            var pred = engine.Predict(input);

            response.Predictions.Add(new PredictedItemInfo
            {
                Item = item.Item,
                DaysUntilNextPurchase = pred.Score,
                PredictedDate = item.LastPurchaseDate?.AddDays(pred.Score)
            });
        }

        return response;
    }

    public List<PredictedItemFromHistory> PredictFromHistory(Guid userId, IEnumerable<Purchase> purchases)
    {
        var result = new List<PredictedItemFromHistory>();

        var events = purchases
            .Where(p => p.UserId == userId)
            .SelectMany(p => p.ListItems.Select(item => new
            {
                UserId = p.UserId,
                Item = item,
                Date = p.Date.Date
            }))
            .ToList();

        if (!events.Any())
            return result;

        var engine = _mlContext.Model.CreatePredictionEngine<PurchaseData, PurchasePrediction>(_model);

        var today = DateTime.Today;

        foreach (var group in events.GroupBy(x => x.Item))
        {
            var ordered = group.OrderBy(x => x.Date).ToList();

            // Pouco histórico: ignora o item pois não tem comportamento previsível.
            if (ordered.Count < 2)
                continue;
            

            const int window = 6;
            if (ordered.Count > window)
                ordered = ordered.Skip(ordered.Count - window).ToList();

            var intervals = new List<float>();
            for (int i = 0; i < ordered.Count - 1; i++)
            {
                float diff = (float)(ordered[i + 1].Date - ordered[i].Date).TotalDays;
                if (diff < 1) diff = 1;
                intervals.Add(diff);
            }

            // Mediana e filtro de outliers (já existia)
            var sorted = intervals.OrderBy(x => x).ToList();
            float median = sorted[sorted.Count / 2];
            float min = median * 0.5f;
            float max = median * 1.5f;

            var filtered = intervals.Where(x => x >= min && x <= max).ToList();
            if (!filtered.Any())
                filtered = intervals;

            float avgInterval = filtered.Average();
            float lastInterval = filtered.Last();

            // ===== NOVO: checar se o padrão é MUITO irregular (outlier/aleatório) =====
            // Coeficiente de variação simples (desvio padrão / média)
            if (filtered.Count >= 2)
            {
                var variance = filtered
                    .Select(x => (x - avgInterval) * (x - avgInterval))
                    .Average();
                float stdDev = (float)Math.Sqrt(variance);
                float cv = stdDev / avgInterval;

                // Se variar demais (ex: > 60%), considera padrão aleatório e NÃO prevê
                if (cv > 0.6f)
                {
                    // Outlier: não retorna nada pra esse item
                    continue;
                }
            }

            var lastDate = ordered.Last().Date;

            var input = new PurchaseData
            {
                UserId = userId.ToString(),
                ItemId = group.Key,
                LastIntervalDays = lastInterval,
                AverageIntervalUserItem = avgInterval,
                PurchaseDayOfWeek = (float)lastDate.DayOfWeek
            };

            var pred = engine.Predict(input);

            if (float.IsNaN(pred.Score) || pred.Score < 0)
                continue;

            var predictedDate = lastDate.AddDays(pred.Score).Date;

            // ===== NOVO: filtrar previsões muito no passado =====
            // tolera atrasos de até 4 dias
            var daysFromToday = (predictedDate - today).TotalDays;

            // se a data prevista é mais antiga do que "hoje - 4 dias", ignora
            if (daysFromToday < -4)
            {
                continue;
            }

            // FUTURO: qualquer valor é aceito (mesmo > 7 dias), conforme você pediu

            result.Add(new PredictedItemFromHistory
            {
                Item = group.Key,
                LastPurchaseDate = lastDate,
                DaysUntilNextPurchase = pred.Score,
                PredictedDate = predictedDate
            });
        }

        return result;
    }

}
