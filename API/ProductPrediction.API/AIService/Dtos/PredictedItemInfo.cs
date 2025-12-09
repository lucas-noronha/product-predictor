namespace ProductPrediction.API.AIService.Dtos
{
    public class PredictedItemInfo
    {
        public string Item { get; set; } = default!;
        public float DaysUntilNextPurchase { get; set; }
        public DateTime? PredictedDate { get; set; }
    }

}
