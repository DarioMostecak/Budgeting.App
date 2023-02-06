namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class CategoryViewValidationException : Exception
    {
        public CategoryViewValidationException(Exception innerException)
            : base(message: "Category view validation error occured, try again.", innerException) { }
    }
}
