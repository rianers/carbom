namespace CarBom.Responses
{
    public class OrderedServiceResponse : Result
    {
        public string Name { get; set; }
        public string Mechanic { get; set; }
        public string CreatedDate { get; set; }
        public string FormattedDate { get; set; }
    }
}
