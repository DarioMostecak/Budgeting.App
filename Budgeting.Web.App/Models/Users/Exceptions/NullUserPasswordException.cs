using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Users.Exceptions
{
    public class NullUserPasswordException : ExceptionModel
    {
        public NullUserPasswordException()
            : base(message: "User password is null.")
        { }
    }
}
