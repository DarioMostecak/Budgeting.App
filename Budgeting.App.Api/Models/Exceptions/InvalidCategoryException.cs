using Budgeting.App.Api.Models.Exceptions.ErrorMessages;

namespace Budgeting.App.Api.Models.Exceptions
{
    public class InvalidCategoryException : Exception
    {

        public InvalidCategoryException()
            : base(message: CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorMessage) { }

        public InvalidCategoryException(Guid parameterId)
            : base(message: string.Format(CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorOneParametersMessage, parameterId)) { }


        public InvalidCategoryException(string parameterName, object parameterValue)
            : base(message: string.Format(CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorTwoParametersMessage, parameterName, parameterValue)) { }


        public List<(string, string)> ValidationErrors = new List<(string, string)>();
    }
}
