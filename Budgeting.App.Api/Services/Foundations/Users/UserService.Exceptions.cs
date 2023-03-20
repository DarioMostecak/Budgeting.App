using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using MongoDB.Driver;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunctions();
        private delegate IQueryable<User> ReturningUsersFunctions();

        private async ValueTask<User> TryCatch(
            ReturningUserFunctions returningUserFunctions)
        {
            try
            {
                return await returningUserFunctions();
            }
            catch (MongoWriteException mongoWriteException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(mongoWriteException);

                throw CreateAndLogValidationException(alreadyExistsUserException);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private IQueryable<User> TryCatch(
            ReturningUsersFunctions returningUsersFunctions)
        {
            try
            {
                return returningUsersFunctions();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private UserValidationException CreateAndLogValidationException(Exception exception)
        {
            var userValidationException =
                new UserValidationException(exception, exception.Data);

            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }
    }
}
