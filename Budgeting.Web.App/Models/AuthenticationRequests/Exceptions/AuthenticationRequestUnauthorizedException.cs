using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class AuthenticationRequestUnauthorizedException : ExceptionModel
    {
        public AuthenticationRequestUnauthorizedException(Exception innerException)
            : base(message: "Fail to authenticate authentication request.", innerException)
        { }
    }
}
