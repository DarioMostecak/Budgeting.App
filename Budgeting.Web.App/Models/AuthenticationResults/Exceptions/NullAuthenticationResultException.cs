using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.AuthenticationResults.Exceptions
{
    public class NullAuthenticationResultException : ExceptionModel
    {
        public NullAuthenticationResultException()
            : base(message: "Authentication result is null, contact support.")
        { }
    }
}
