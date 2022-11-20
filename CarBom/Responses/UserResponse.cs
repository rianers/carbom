namespace CarBom.Responses
{
    public class UserResponse : Result
    {
        public string UserId { get; set; }
        public bool isValid { get; set; }
    }
}
