namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException()
            : base(message: "The category is null.") { }
    }
}
