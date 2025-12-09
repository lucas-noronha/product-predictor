namespace ProductPrediction.API.AIService.Dtos
{

    public class PredictedItemFromHistory
    {
        public string Item { get; set; } = default!;
        public float? DaysUntilNextPurchase { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public DateTime? PredictedDate { get; set; }
    }


}
