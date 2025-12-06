namespace ProductPrediction.API.AIService.Dtos
{
    public class PurchasePredictionResponse
    {
        public float DaysUntilNextPurchase { get; set; }
        public DateTime? PredictedDate { get; set; }
    }


}
