namespace ProductPrediction.API.AIService.Dtos
{
    public class BatchPurchasePredictionRequest
    {
        public Guid UserId { get; set; }
        public List<ItemPredictionFeature> Items { get; set; } = new();
    }


}
