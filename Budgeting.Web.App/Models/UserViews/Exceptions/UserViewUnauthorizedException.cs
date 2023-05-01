using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.UserViews.Exceptions
{
    public class UserViewUnauthorizedException : ExceptionModel
    {
        public UserViewUnauthorizedException(Exception innerException)
            : base(message: "Unauthorized, can't access data.", innerException) { }
    }
}
