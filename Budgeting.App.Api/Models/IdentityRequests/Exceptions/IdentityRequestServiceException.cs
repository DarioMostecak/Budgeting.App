namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class IdentityRequestServiceException : Exception
    {
        public IdentityRequestServiceException(Exception innerException) :
            base(message: "Service exception contact support.", innerException)
        { }



    }
}
