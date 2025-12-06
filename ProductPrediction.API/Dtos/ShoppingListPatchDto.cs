namespace ProductPrediction.API.Dtos
{
    public record ShoppingListPatchDto(string? Title, List<string>? Items);
}
