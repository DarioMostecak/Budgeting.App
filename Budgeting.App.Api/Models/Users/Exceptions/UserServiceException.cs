namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class UserServiceException : Exception
    {
        public UserServiceException(Exception innerException)
            : base(message: "Service error occurred, contact support.", innerException)
        { }
    }
}
