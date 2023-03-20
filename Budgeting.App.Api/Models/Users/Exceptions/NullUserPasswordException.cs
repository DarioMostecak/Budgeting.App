namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class NullUserPasswordException : Exception
    {
        public NullUserPasswordException() :
            base(message: "User password is null.")
        { }

    }
}
