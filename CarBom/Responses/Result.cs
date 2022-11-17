namespace CarBom.Responses
{
    public class Result
    {
        public string? ResultCode { get; set; }
        public List<ResultDetail>? ResultDetails { get; set; }
    }

    public class ResultDetail
    {
        public string? Message { get; set; }
    }
}
