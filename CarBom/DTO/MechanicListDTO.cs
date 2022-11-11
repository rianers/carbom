namespace CarBom.DTO
{
    public record MechanicListDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Services { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
    }
}
