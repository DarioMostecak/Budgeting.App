using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class FailedLoginVewDependencyException : ExceptionModel
    {
        public FailedLoginVewDependencyException(Exception innerException)
            : base(message: "Failed dependency error occured. Try again or contact support.", innerException)
        { }
    }
}
