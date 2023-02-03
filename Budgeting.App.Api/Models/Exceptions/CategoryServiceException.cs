using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public CategoryServiceException(Exception innerException)
            : base(message: CategoryExceptionErrorMessages.CategoryServiceExceptionErrorMessage, innerException) { }
    }
}
