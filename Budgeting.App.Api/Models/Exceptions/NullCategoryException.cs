namespace Budgeting.App.Api.Models.Exceptions
{
    public class NullCategoryException : Exception
    {
        public NullCategoryException()
            : base(message: "The categpry is null.") { }
    }
}
