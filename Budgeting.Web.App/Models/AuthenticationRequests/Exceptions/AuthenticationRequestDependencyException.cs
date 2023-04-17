using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class AuthenticationRequestDependencyException : ExceptionModel
    {
        public AuthenticationRequestDependencyException(Exception innerException)
            : base(message: "Authentication request dependency error occured, contact support.", innerException)
        { }
    }
}
