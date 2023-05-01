using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class UserDependencyValidationException : ExceptionModel
    {
        public UserDependencyValidationException(Exception innerException) :
            base(message: "User dependency validation error occured. Try again.", innerException)
        { }
    }
}
