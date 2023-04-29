using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class UserServiceException : ExceptionModel
    {
        public UserServiceException(Exception innerException)
            : base(message: "Service exception occured. Contact support.", innerException)
        { }
    }
}
