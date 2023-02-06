namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException()
            : base(message: "Category is null.") { }
    }
}
