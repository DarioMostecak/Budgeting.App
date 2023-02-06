namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class CategoryViewDependencyException : Exception
    {
        public CategoryViewDependencyException(Exception innerException)
            : base(message: "Category view dependency error occured, contact support.", innerException) { }
    }
}
