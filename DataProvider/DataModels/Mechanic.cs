namespace DataProvider.DataModels
{
    public class Mechanic
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double? Ranking { get; set; }
        public double Distance { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public List<Service>? Services { get; set; }
    }
}
