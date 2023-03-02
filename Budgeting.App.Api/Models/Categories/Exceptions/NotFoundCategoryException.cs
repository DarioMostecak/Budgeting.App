namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException(Guid categoryId)
            : base(message: string.Format($"Couldn't find category with id: {categoryId}.")) { }

    }
}
