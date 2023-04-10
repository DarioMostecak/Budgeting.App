namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class IdentityRequestDependencyException : Exception
    {
        public IdentityRequestDependencyException(Exception innerException)
            : base(message: "Service exception contact support.", innerException)
        { }
    }
}
