using ProductPrediction.API.Entities; // onde está Purchase
using ProductPrediction.API.AIService.Dtos;

namespace ProductPrediction.API.AIService.Interfaces;

public interface IPurchasePredictionService
{
    PurchasePredictionResponse PredictSingle(PurchasePredictionRequest request);
    BatchPurchasePredictionResponse PredictBatch(BatchPurchasePredictionRequest request);
    List<PredictedItemFromHistory> PredictFromHistory(Guid userId, IEnumerable<Purchase> purchases);
}
