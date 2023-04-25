using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class LoginViewUnauthorizeException : ExceptionModel
    {
        public LoginViewUnauthorizeException(Exception innerException)
            : base(message: "Fail to log in. Invalid password or email.", innerException)
        { }
    }
}
