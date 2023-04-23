using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationRequests.Exceptions
{
    public class InvalidAuthenticationRequestException : ExceptionModel
    {
        public InvalidAuthenticationRequestException()
            : base(message: "Invalid authentication request error occurred, please fix errors and try again.")
        { }
    }
}
