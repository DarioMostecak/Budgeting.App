using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class CategoryServiceException : Exception
    {
        public CategoryServiceException(Exception innerException)
            : base(message: CategoryExceptionErrorMessages.CategoryServiceExceptionErrorMessage, innerException) { }
    }
}
