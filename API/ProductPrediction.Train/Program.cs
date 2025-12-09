using System.Globalization;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using ProductPrediction.Train.Models;

// =========================================================
// 1. Criação do MLContext (ponto de entrada do ML.NET)
// =========================================================
var mlContext = new MLContext();

// =========================================================
// 2. Carregamento do arquivo CSV bruto (uid, date, item)
//    - Aqui o arquivo ainda não está no formato ideal
//    - Lemos usando a classe RawPurchase
// =========================================================
var dataPath = "data.csv";

var rawDataView = mlContext.Data.LoadFromTextFile<RawPurchase>(
    path: dataPath,
    hasHeader: true,
    separatorChar: ',');

var rawEnum = mlContext.Data.CreateEnumerable<RawPurchase>(
    rawDataView,
    reuseRowObject: false);

Console.WriteLine("Arquivo carregado.");

// =========================================================
// 3. Conversão de string -> DateTime e normalização básica
//    - Transformamos o CSV cru em uma estrutura com:
//      UserId, ItemId, Date (DateTime)
// =========================================================
var parsed = rawEnum
    .Select(x => new
    {
        UserId = x.uid,
        ItemId = x.item,
        Date = DateTime.ParseExact(
            x.date,
            "dd/MM/yyyy",
            CultureInfo.GetCultureInfo("pt-BR"))
    });

// =========================================================
// 4. Preparação do dataset de treino com histórico
//    - Agrupamos por (UserId, ItemId)
//    - Ordenamos as compras por data
//    - Calculamos intervalos entre compras
//    - Calculamos mediana de intervalos (padrão típico)
//    - Identificamos e removemos OUTLIERS nos intervalos
//    - Geramos registros de treino com features + label
// =========================================================
var purchases = parsed
    .GroupBy(x => new { x.UserId, x.ItemId }) // agrupa por usuário+item
    .SelectMany(g =>
    {
        var ordered = g.OrderBy(p => p.Date).ToList();
        var list = new List<PurchaseData>();

        // precisa ter pelo menos 3 compras para termos:
        // - uma compra anterior (prev)
        // - uma compra atual (current)
        // - uma próxima compra (next)
        if (ordered.Count < 3)
            return list;

        // -------------------------------------------------
        // 4.1. Calcular todos os intervalos entre compras
        //      Ex: [7, 8, 6, 7, 2, 1, 7]
        // -------------------------------------------------
        var allIntervals = new List<float>();
        for (int i = 0; i < ordered.Count - 1; i++)
        {
            var interval = (float)(ordered[i + 1].Date - ordered[i].Date).TotalDays;
            allIntervals.Add(interval);
        }

        // -------------------------------------------------
        // 4.2. Calcular a MEDIANA dos intervalos
        //      - Representa o "padrão típico" desse user+item
        // -------------------------------------------------
        var orderedIntervals = allIntervals.OrderBy(x => x).ToList();
        float median = orderedIntervals[orderedIntervals.Count / 2];

        // -------------------------------------------------
        // 4.3. Definir faixa aceitável de variação
        //      - Ex: 50% para baixo e 50% para cima da mediana
        //      - Intervalos fora dessa faixa são considerados outliers
        // -------------------------------------------------
        float minAccepted = median * 0.5f;  // abaixo de metade do normal → outlier
        float maxAccepted = median * 1.5f;  // acima de 1.5x do normal → outlier

        // média geral (sem filtrar outlier) só como feature adicional
        float avgInterval = allIntervals.Average();

        // -------------------------------------------------
        // 4.4. Gerar exemplos de treino
        //      - Cada exemplo usa:
        //          prev → current → next
        //      - Features:
        //          LastIntervalDays, AverageIntervalUserItem, PurchaseDayOfWeek
        //      - Label:
        //          DaysUntilNextPurchase = intervalo current→next
        //      - Se o próximo intervalo for outlier, ignoramos esse exemplo
        // -------------------------------------------------
        for (int i = 1; i < ordered.Count - 1; i++)
        {
            var prev = ordered[i - 1];
            var current = ordered[i];
            var next = ordered[i + 1];

            var lastInterval = (float)(current.Date - prev.Date).TotalDays;
            var nextInterval = (float)(next.Date - current.Date).TotalDays;

            // se o próximo intervalo for outlier, NÃO usamos esse registro para treino
            if (nextInterval < minAccepted || nextInterval > maxAccepted)
                continue;

            list.Add(new PurchaseData
            {
                UserId = current.UserId,
                ItemId = current.ItemId,
                LastIntervalDays = lastInterval,
                AverageIntervalUserItem = avgInterval,
                PurchaseDayOfWeek = (float)current.Date.DayOfWeek,
                DaysUntilNextPurchase = nextInterval
            });
        }

        return list;
    })
    .ToList();

Console.WriteLine($"Registros com label (DaysUntilNextPurchase) após filtragem de outliers: {purchases.Count}");

// =========================================================
// 5. Conversão da lista de PurchaseData para IDataView
//    - É o formato de dados interno que o ML.NET usa
// =========================================================
var dataView = mlContext.Data.LoadFromEnumerable(purchases);

// =========================================================
// 6. Definição do pipeline de ML
//    - OneHotEncoding de UserId e ItemId (transforma em vetores numéricos)
//    - Concatenação de todas as features
//    - Treinador de regressão FastTree
// =========================================================
var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(
        new[]
        {
            new InputOutputColumnPair("UserIdEncoded", nameof(PurchaseData.UserId)),
            new InputOutputColumnPair("ItemIdEncoded", nameof(PurchaseData.ItemId))
        })
    .Append(mlContext.Transforms.Concatenate(
        "Features",
        "UserIdEncoded",
        "ItemIdEncoded",
        nameof(PurchaseData.LastIntervalDays),
        nameof(PurchaseData.AverageIntervalUserItem),
        nameof(PurchaseData.PurchaseDayOfWeek)))
    .Append(mlContext.Regression.Trainers.FastTree(
        labelColumnName: nameof(PurchaseData.DaysUntilNextPurchase),
        featureColumnName: "Features"));

// =========================================================
// 7. Divisão dos dados em treino e teste (Train/Test Split)
//    - 80% para treino
//    - 20% para teste/avaliação
// =========================================================
var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

// =========================================================
// 8. Treinamento do modelo
// =========================================================
Console.WriteLine("Treinando modelo...");
var model = pipeline.Fit(split.TrainSet);

// =========================================================
// 9. Avaliação do modelo
//    - Usamos o conjunto de teste (TestSet)
//    - Métricas principais:
//        R²  -> quão bem o modelo explica a variação dos dados
//        MAE -> erro médio absoluto em dias
// =========================================================
var predictions = model.Transform(split.TestSet);

var metrics = mlContext.Regression.Evaluate(
    predictions,
    labelColumnName: nameof(PurchaseData.DaysUntilNextPurchase),
    scoreColumnName: "Score");

Console.WriteLine($"R²: {metrics.RSquared}");
Console.WriteLine($"MAE: {metrics.MeanAbsoluteError}");

// =========================================================
// 10. Salvando o modelo treinado em disco
//     - Gera o arquivo model.zip
//     - Esse arquivo será carregado pela API depois
// =========================================================
var modelPath = "model.zip";
mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
Console.WriteLine($"Modelo salvo em: {modelPath}");
    