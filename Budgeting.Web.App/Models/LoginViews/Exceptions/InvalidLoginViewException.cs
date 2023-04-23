using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class InvalidLoginViewException : ExceptionModel
    {
        public InvalidLoginViewException()
            : base(message: "Invalid login view request error occurred, please fix errors and try again.")
        { }

        public InvalidLoginViewException(string parameterName, object parameterValue)
            : base(message: $"Invalid login, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
