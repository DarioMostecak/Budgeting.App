using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class FailedAuthenticationRequestDependencyException : ExceptionModel
    {
        public FailedAuthenticationRequestDependencyException(Exception innerException)
            : base(message: "Failed authentication request dependency error occured.", innerException)
        { }
    }
}
