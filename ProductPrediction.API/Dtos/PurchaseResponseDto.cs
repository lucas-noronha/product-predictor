namespace ProductPrediction.API.Dtos
{
    public record PurchaseResponseDto(Guid Id, Guid UserId, DateTime Date, List<string> items);
}
