using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException() { }

        public NotFoundCategoryException(Guid id)
            : base(message: string.Format(CategoryExceptionErrorMessages.CategoryNotFoundExceptionErrorMessage, id)) { }

    }
}
