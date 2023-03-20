namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class UserDependencyException : Exception
    {
        public UserDependencyException(Exception innerException)
            : base(message: "Service dependency error occurred, contact support.", innerException) { }
    }
}
