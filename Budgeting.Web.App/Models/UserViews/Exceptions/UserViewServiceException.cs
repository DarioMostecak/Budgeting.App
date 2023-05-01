using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class UserViewServiceException : ExceptionModel
    {
        public UserViewServiceException(Exception innerException)
            : base(message: "Service error occured, contact support.", innerException) { }
    }
}
