using Budgeting.Web.App.Models.Exceptions.ErrorMessages;

namespace Budgeting.Web.App.Models.Exceptions
{
    public class InvalidCategoryException : Exception
    {

        public InvalidCategoryException()
            : base(message: CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorMessage)
        {
            this.Errors = new List<(string, string)>();
        }

        public InvalidCategoryException(Guid parameterId)
            : base(message: string.Format(CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorOneParametersMessage, parameterId))
        {
            this.Errors = new List<(string, string)>();
        }

        public InvalidCategoryException(string parameterName, object parameterValue)
            : base(message: string.Format(CategoryExceptionErrorMessages.InvalidCategoryExceptionErrorTwoParametersMessage, parameterName, parameterValue))
        {
            this.Errors = new List<(string, string)>();
        }

        public List<(string, string)> Errors { get; set; }




    }
}
