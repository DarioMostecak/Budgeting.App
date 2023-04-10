namespace Budgeting.App.Api.Models.IdentityRequests.Exceptions
{
    public class FailAuthenticationIdentityRequestException : Exception
    {
        public FailAuthenticationIdentityRequestException(string message)
            : base(message: $"Identity fail to authenticate password or email: {message}.")
        { }
    }
}
