namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class NullAuthenticationRequestException : Exception
    {
        public NullAuthenticationRequestException()
            : base(message: "Authentication request is null.") { }
    }
}
