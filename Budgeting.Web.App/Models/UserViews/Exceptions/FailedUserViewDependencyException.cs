using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class FailedUserViewDependencyException : ExceptionModel
    {
        public FailedUserViewDependencyException(Exception innerException)
            : base(message: "Failed dependency error occured. Contact support.", innerException)
        { }
    }
}
