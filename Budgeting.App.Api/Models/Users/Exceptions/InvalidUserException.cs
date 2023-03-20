using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class InvalidUserException : ExceptionModel
    {
        public InvalidUserException()
            : base(message: "Invalid user. Please fix errors and try again.") { }

        public InvalidUserException(string parameterName, object parameterValue)
            : base(message: $"Invalid user" +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
