using Budgeting.Web.App.Models.Users.Exceptions;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;

namespace Budgeting.Web.App.Services.Views.UserViews
{
    public partial class UserViewService
    {
        private delegate ValueTask<UserView> ReturningUserViewFunctions();
        private delegate void ReturnigNothingFunction();

        private async ValueTask<UserView> TryCatch(
            ReturningUserViewFunctions returningUserViewFunctions)
        {
            try
            {
                return await returningUserViewFunctions();
            }
            catch (NullUserViewException nullUserViewException)
            {
                throw CreateAndLogValidationException(nullUserViewException);
            }
            catch (InvalidUserViewException invalidUserViewException)
            {
                throw CreateAndLogValidationException(invalidUserViewException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (UserUnauthorizedException userUnauthorizedException)
            {
                throw CreateAndLogUnauthorizedException(userUnauthorizedException);
            }
            catch (UserDependencyException userDependencyException)
            {
                var failedUserViewDependencyException =
                    new FailedUserViewDependencyException(userDependencyException);

                throw CreateAndLogDependencyException(failedUserViewDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                var failedUserViewServiceException =
                    new FailedUserViewServiceException(userServiceException);

                throw CreateAndLogServiceException(failedUserViewServiceException);
            }
            catch (Exception exception)
            {
                var failedViewServiceException =
                    new FailedUserViewServiceException(exception);

                throw CreateAndLogServiceException(failedViewServiceException);
            }
        }

        private void TryCatch(ReturnigNothingFunction returnigNothingFunction)
        {
            try
            {
                returnigNothingFunction();
            }
            catch (InvalidUserViewException invalidUserViewException)
            {
                throw CreateAndLogValidationException(invalidUserViewException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private UserViewValidationException CreateAndLogValidationException(Exception exception)
        {
            var userViewValidationException =
                new UserViewValidationException(exception, exception.Data);

            this.loggingBroker.LogError(userViewValidationException);

            return userViewValidationException;
        }

        private UserViewDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var userViewDependencyValidationException =
               new UserViewDependencyValidationException(exception);

            this.loggingBroker.LogError(userViewDependencyValidationException);

            return userViewDependencyValidationException;
        }

        private UserViewDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var userDependencyException =
                new UserViewDependencyException(exception);

            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserViewServiceException CreateAndLogServiceException(Exception exception)
        {
            var userViewServiceException =
                new UserViewServiceException(exception);

            this.loggingBroker.LogError(userViewServiceException);

            return userViewServiceException;
        }

        private UserViewUnauthorizedException CreateAndLogUnauthorizedException(Exception exception)
        {
            var userViewUnauthorizedException =
                new UserViewUnauthorizedException(exception);

            this.loggingBroker.LogError(userViewUnauthorizedException);

            return userViewUnauthorizedException;
        }
    }
}
