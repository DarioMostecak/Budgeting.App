using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class UserDependencyException : ExceptionModel
    {
        public UserDependencyException(Exception innerException)
            : base(message: "Dependency exception occured. Contact support", innerException)
        { }
    }
}
