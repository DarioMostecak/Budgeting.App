using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class FailedUserServiceException : ExceptionModel
    {
        public FailedUserServiceException(Exception innerException)
            : base(message: "Failed service exception occured. Contact support.", innerException)
        { }
    }
}
