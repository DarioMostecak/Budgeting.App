namespace Budgeting.Web.App.Models.CategoryViews.Exceptions
{
    public class NullCategoryViewException : Exception
    {
        public NullCategoryViewException()
            : base(message: "CategoryView is null.") { }
    }
}
