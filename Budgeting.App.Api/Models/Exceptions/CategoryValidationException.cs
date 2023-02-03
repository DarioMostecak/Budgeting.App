namespace Budgeting.App.Api.Models.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException)
        { }

        public List<string> ValidationErrorMessages = new List<string>();
    }
}
