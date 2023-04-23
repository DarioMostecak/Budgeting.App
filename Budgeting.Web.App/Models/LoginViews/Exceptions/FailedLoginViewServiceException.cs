using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.LoginViews.Exceptions
{
    public class FailedLoginViewServiceException : ExceptionModel
    {
        public FailedLoginViewServiceException(Exception innerException)
            : base(message: "Failed service exception occured, try again or contact support.", innerException)
        {

        }
    }
}
