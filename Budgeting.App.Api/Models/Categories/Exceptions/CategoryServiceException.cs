namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public CategoryServiceException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException) { }
    }
}
