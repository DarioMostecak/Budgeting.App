namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class CategoryViewServiceException : Exception
    {
        public CategoryViewServiceException(Exception innerException)
            : base(message: "Category view service error occured, contact support", innerException) { }
    }
}
