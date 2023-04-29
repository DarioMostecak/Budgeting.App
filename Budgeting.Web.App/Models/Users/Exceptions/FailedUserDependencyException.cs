using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class FailedUserDependencyException : ExceptionModel
    {
        public FailedUserDependencyException(Exception innerException)
            : base(message: "Failed dependency exception occured. Contact support.", innerException)
        { }
    }
}
