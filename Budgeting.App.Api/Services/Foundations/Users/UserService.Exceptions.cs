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
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (NullUserPasswordException nullUserPasswordException)
            {
                throw CreateAndLogValidationException(nullUserPasswordException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (AlreadyExistsUserException alreadyExsistUserException)
            {
                throw CreateAndLogValidationException(alreadyExsistUserException);
            }
            catch (NotFoundUserException notFoundUserException)
            {
                throw CreateAndLogValidationException(notFoundUserException);
            }
            catch (MongoWriteException mongoWriteException)
            {
                var alreadyExistsUserException =
                    new AlreadyExistsUserException(mongoWriteException);

                throw CreateAndLogValidationException(alreadyExistsUserException);
            }
            catch (MongoException mongoException)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(mongoException);

                throw CreateAndLogDependencyException(failedUserServiceException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
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

        private UserDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var userDependencyException =
                new UserDependencyException(exception);

            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Exception exception)
        {
            var userServiceException =
                new UserServiceException(exception);

            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}
