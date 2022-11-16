using DataProvider.DataModels;

namespace CarBom.Requests
{
    public class MechanicRequest
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Ranking { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
    }
}
