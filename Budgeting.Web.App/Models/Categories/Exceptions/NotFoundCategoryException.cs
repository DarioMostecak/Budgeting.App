namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException() { }

        public NotFoundCategoryException(Guid parameterId)
            : base(message: $"Category with {parameterId} not found.") { }

    }
}
