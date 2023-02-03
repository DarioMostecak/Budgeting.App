using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException() { }

        public NotFoundCategoryException(Guid id)
            : base(message: string.Format(CategoryExceptionErrorMessages.CategoryNotFoundExceptionErrorMessage, id)) { }

    }
}
