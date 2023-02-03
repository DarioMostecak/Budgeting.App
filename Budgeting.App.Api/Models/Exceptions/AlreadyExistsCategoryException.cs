using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class AlreadyExistsCategoryException : Exception
    {
        public AlreadyExistsCategoryException(Exception innerException)
            : base(message: CategoryExceptionErrorMessages.AlredyExistsCategoryExceptionErrorMessage, innerException) { }
    }
}
