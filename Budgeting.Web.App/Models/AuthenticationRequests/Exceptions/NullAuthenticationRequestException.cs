using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class NullAuthenticationRequestException : ExceptionModel
    {
        public NullAuthenticationRequestException()
            : base(message: "Authentication request is null.") { }
    }
}
