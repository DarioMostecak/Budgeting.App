namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class AlreadyExistsUserException : Exception
    {
        public AlreadyExistsUserException(Exception innerException)
            : base(message: "User already exist.", innerException)
        { }

        public AlreadyExistsUserException()
            : base(message: "User email already exist.") { }
    }
}
