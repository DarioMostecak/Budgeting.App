using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(Exception innerException)
            : base(message: CategoryExceptionErrorMessages.CategoryValidationExceptionErrorMessage, innerException)
        { }
    }
}
