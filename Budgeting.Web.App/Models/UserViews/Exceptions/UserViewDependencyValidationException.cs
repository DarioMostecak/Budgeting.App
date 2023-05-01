using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class UserViewDependencyValidationException : ExceptionModel
    {
        public UserViewDependencyValidationException(Exception innerException)
            : base(message: "User input not valid, please fix errors and try again. ", innerException)
        { }
    }
}
