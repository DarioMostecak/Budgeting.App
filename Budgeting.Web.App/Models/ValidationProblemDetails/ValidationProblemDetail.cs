namespace Budgeting.Web.App.Models.ValidationProblemDetails
{
    public class ValidationProblemDetail
    {
        public int Status { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public IDictionary<string, string[]>? Errors { get; }
    }
}
