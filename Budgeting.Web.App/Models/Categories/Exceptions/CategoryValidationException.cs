namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException)
        { }
    }
}
