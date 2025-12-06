namespace ProductPrediction.Train.Models;

public class PurchaseData
{
    public string UserId { get; set; }
    public string ItemId { get; set; }

    // features derivadas do histórico
    public float LastIntervalDays { get; set; }          // último intervalo entre compras
    public float AverageIntervalUserItem { get; set; }   // média de intervalo para esse user+item
    public float PurchaseDayOfWeek { get; set; }         // dia da semana da compra atual (0–6)

    // label
    public float DaysUntilNextPurchase { get; set; }     // o que o modelo vai prever
}
