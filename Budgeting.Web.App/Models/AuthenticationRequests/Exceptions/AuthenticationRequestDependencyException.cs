namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class AuthenticationRequestDependencyException : Exception
    {
        public AuthenticationRequestDependencyException(Exception innerException)
            : base(message: "Authentication request dependency error occured, contact support.", innerException)
        { }
    }
}
