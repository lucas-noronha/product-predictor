namespace ProductPrediction.API.Dtos
{
    public record ShoppingListResponseDto(Guid Id, Guid UserId, string Title, List<string> items);
}
