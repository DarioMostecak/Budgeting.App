using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class UserUnauthorizedException : ExceptionModel
    {
        public UserUnauthorizedException(Exception innerException)
            : base(message: "Unauthorized, can't access data.", innerException) { }
    }
}
