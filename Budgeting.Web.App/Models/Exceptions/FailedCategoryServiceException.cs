using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class FailedCategoryServiceException : Exception
    {
        public FailedCategoryServiceException(Exception innerException) :
           base(message: CategoryExceptionErrorMessages.FailedCategoryServiceExceptionErrorMessage, innerException)
        { }
    }
}
