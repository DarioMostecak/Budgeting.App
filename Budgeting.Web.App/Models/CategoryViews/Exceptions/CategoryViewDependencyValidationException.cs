namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class CategoryViewDependencyValidationException : Exception
    {
        public CategoryViewDependencyValidationException(Exception innerException)
        : base(message: "Category view dependency validation error occured.", innerException) { }
    }
}
