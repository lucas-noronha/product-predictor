namespace ProductPrediction.API.AIService.Dtos
{
    public class ItemPredictionFeature
    {
        public string Item { get; set; } = default!;
        public float LastIntervalDays { get; set; }
        public float AverageIntervalUserItem { get; set; }
        public float PurchaseDayOfWeek { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
    }


}
