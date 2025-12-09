namespace ProductPrediction.API.Dtos
{
    public record ShoppingListCreateDto(string UserId, string Title, List<string> Items);
}
