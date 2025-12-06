namespace ProductPrediction.API.AIService.Dtos
{
    public class BatchPurchasePredictionResponse
    {
        public Guid UserId { get; set; }
        public List<PredictedItemInfo> Predictions { get; set; } = new();
    }


}
