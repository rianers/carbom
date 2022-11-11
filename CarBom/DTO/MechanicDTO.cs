using DataProvider.DataModels;

namespace CarBom.DTO
{
    public class MechanicDTO
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double Ranking { get; set; }
        public double Distance { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
    }
}
