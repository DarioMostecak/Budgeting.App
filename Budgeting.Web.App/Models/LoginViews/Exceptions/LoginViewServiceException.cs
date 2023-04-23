using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class LoginViewServiceException : ExceptionModel
    {
        public LoginViewServiceException(Exception innerException)
            : base(message: "Service error occured, try again or contact support.", innerException)
        { }
    }
}
