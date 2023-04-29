using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;

namespace Budgeting.Web.App.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> UserReturnigFunction();

        private async ValueTask<User> TryCatch(
            UserReturnigFunction userReturnigFunction)
        {
            try
            {
                return await userReturnigFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizeException)
            {
                var failedUserUnauthorizedException =
                    new FailedUserUnauthorizedException(httpResponseUnauthorizeException);

                throw CreateAndLogUnauthorizedException(failedUserUnauthorizedException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var failUserDependencyError =
                    new FailedUserDependencyException(httpResponseNotFoundException);

                throw CreateAndLogDependencyException(failUserDependencyError);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var failedUserDependencyException =
                    new FailedUserDependencyException(httpResponseBadRequestException);

                throw CreateAndLogDependencyException(failedUserDependencyException);
            }
            catch (HttpResponseInternalServerErrorException httpResponseInternalServerErrorException)
            {
                var failedUserDependencyException =
                    new FailedUserDependencyException(httpResponseInternalServerErrorException);

                throw CreateAndLogDependencyException(failedUserDependencyException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedUserDependencyException =
                    new FailedUserDependencyException(httpRequestException);

                throw CreateAndLogDependencyException(failedUserDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedUserDependencyEception =
                    new FailedUserDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedUserDependencyEception);
            }
            catch (Exception exception)
            {
                var failedUserServiceException =
                    new FailedUserServiceException(exception);
                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserValidationException CreateAndLogValidationException(Exception exception)
        {
            var userValidationException = new UserValidationException(exception, exception.Data);
            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }

        private UserDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Exception exception)
        {
            var userServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }

        private UserUnauthorizedException CreateAndLogUnauthorizedException(Exception exception)
        {
            var userUnauthorizedException = new UserUnauthorizedException(exception);
            this.loggingBroker.LogError(userUnauthorizedException);

            return userUnauthorizedException;
        }
    }
}
