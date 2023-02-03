using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class FailedCategoryServiceException : Exception
    {
        public FailedCategoryServiceException(Exception innerException) :
           base(message: CategoryExceptionErrorMessages.FailedCategoryServiceExceptionErrorMessage, innerException)
        { }
    }
}
