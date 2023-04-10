namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class NullIdentityRequestException : Exception
    {
        public NullIdentityRequestException()
            : base(message: "Identity request is null.") { }



    }
}
