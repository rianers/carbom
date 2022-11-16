namespace CarBom.Requests
{
    public record MechanicListRequest
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Services { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}
