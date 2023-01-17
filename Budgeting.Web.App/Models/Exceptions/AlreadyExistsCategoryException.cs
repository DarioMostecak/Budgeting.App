using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class AlreadyExistsCategoryException : Exception
    {
        public AlreadyExistsCategoryException(Exception innerException)
            : base(message: CategoryExceptionErrorMessages.AlredyExistsCategoryExceptionErrorMessage, innerException) { }
    }
}
