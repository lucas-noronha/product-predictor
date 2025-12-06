namespace ProductPrediction.API.AIService.Dtos;

public class PurchaseData
{
    public string UserId { get; set; } = default!;
    public string ItemId { get; set; } = default!;

    public float LastIntervalDays { get; set; }
    public float AverageIntervalUserItem { get; set; }
    public float PurchaseDayOfWeek { get; set; }

    // label (não usado na previsão)
    public float DaysUntilNextPurchase { get; set; }
}
