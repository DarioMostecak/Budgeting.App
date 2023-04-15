namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class AuthenticationRequestServiceException : Exception
    {
        public AuthenticationRequestServiceException(Exception innerException)
            : base(message: "Authentication request service error occured, contact support.", innerException)
        { }
    }
}
