namespace Budgeting.App.Api.Contracts
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public DateTime Timestamp { get; set; }
    }
}
