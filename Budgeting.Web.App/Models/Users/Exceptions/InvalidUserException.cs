using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class InvalidUserException : ExceptionModel
    {
        public InvalidUserException()
            : base(message: "Invalid user, validation error occurred, please fix errors and try again.")
        { }

        public InvalidUserException(Exception innerException) :
            base(message: "Invalid user data, please try again.", innerException)
        { }

        public InvalidUserException(string parameterName, object parameterValue)
            : base(message: $"Invalid user, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
