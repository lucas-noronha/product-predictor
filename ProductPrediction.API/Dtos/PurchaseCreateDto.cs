namespace ProductPrediction.API.Dtos
{
    public record PurchaseCreateDto(string UserId, DateTime Date, List<string> Items);
}
