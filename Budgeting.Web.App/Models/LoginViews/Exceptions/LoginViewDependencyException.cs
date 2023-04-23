using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class LoginViewDependencyException : ExceptionModel
    {
        public LoginViewDependencyException(Exception innerException)
            : base(message: "Dependency exception occured, try again or contact support.", innerException) { }
    }
}
