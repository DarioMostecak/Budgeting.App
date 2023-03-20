namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException()
            : base(message: "User is null.")
        { }
    }
}
