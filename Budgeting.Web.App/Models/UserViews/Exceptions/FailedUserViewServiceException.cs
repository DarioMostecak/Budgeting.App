using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class FailedUserViewServiceException : ExceptionModel
    {
        public FailedUserViewServiceException(Exception innerException)
            : base(message: "Failed service error occured. Contact support.", innerException)
        { }
    }
}
