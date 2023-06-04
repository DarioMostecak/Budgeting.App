using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class NullUserException : ExceptionModel
    {
        public NullUserException()
            : base(message: "User is null.")
        { }
    }
}
