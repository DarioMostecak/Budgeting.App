namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(Guid id) :
            base(message: $"Couldn't find user with email: {id}.")
        { }

        public NotFoundUserException(string email)
            : base(message: $"Couldn't find user with email: {email}.")
        { }
    }
}
