namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class FailIdentityRequestServiceException : Exception
    {
        public FailIdentityRequestServiceException(Exception innerException)
            : base(message: "Failed identity service exception, contact support.", innerException)
        { }

    }
}
