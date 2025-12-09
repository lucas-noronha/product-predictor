namespace ProductPrediction.API.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public string Items { get; set; }

        public List<string> ListItems => Items.Split(',').Select(x => x.Trim()).ToList();
    }
}
