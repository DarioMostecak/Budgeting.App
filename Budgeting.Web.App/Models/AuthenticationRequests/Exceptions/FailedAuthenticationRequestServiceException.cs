using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class FailedAuthenticationRequestServiceException : ExceptionModel
    {
        public FailedAuthenticationRequestServiceException(Exception innerException)
            : base(message: "Authentication request service error occured, contact support.", innerException)
        { }
    }
}
