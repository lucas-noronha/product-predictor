namespace ProductPrediction.API.Entities
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Items { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<string> ListItems => Items.Split(',').Select(x => x.Trim()).ToList();

    }
}
