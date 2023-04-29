using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class FailedUserUnauthorizedException : ExceptionModel
    {
        public FailedUserUnauthorizedException(Exception innerException)
            : base(message: "Fail unauthorize error.", innerException)
        { }
    }
}
