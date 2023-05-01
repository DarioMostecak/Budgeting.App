using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class UserViewDependencyException : ExceptionModel
    {
        public UserViewDependencyException(Exception innerException)
            : base(message: "Dependency error occured, contact support", innerException)
        { }
    }
}
