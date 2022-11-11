using DataProvider.DataModels;
using System.Text.Json.Serialization;

namespace CarBom.DTO
{
    public class MechanicDTO
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double Ranking { get; set; }
        [JsonIgnore]
        public double Distance { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
    }
}
