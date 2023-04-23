using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews.Exceptions;

namespace Budgeting.Web.App.Services.Views.LoginViews
{
    public partial class LoginViewService
    {
        private delegate ValueTask<AuthenticationResult> LoginViewReturningFunction();
        private delegate void ReturningNothingFunction();

        private async ValueTask<AuthenticationResult> TryCatch(
            LoginViewReturningFunction loginViewReturningFunction)
        {
            try
            {
                return await loginViewReturningFunction();
            }
            catch (NullLoginViewException nullLoginViewException)
            {
                throw CreateAndLogValidationException(nullLoginViewException);
            }
            catch (InvalidLoginViewException invalidLoginViewException)
            {
                throw CreateAndLogValidationException(invalidLoginViewException);
            }
            catch (AuthenticationRequestValidationException authenticationRequestValidationException)
            {
                throw CreateAndLogValidationException(authenticationRequestValidationException);
            }
            catch (AuthenticationRequestDependencyException authenticationRequestDependencyException)
            {
                var failedLoginViewDependencyException =
                    new FailedLoginVewDependencyException(authenticationRequestDependencyException);

                throw CreateAndLogDependencyException(failedLoginViewDependencyException);
            }
            catch (AuthenticationRequestServiceException authenticationRequestServiceException)
            {
                var failedLoginViewServiceException =
                    new FailedLoginViewServiceException(authenticationRequestServiceException);

                throw CreateAndLogServiceException(failedLoginViewServiceException);
            }
            catch (Exception exception)
            {
                var failedLoginViewServiceException =
                    new FailedLoginViewServiceException(exception);

                throw CreateAndLogServiceException(failedLoginViewServiceException);
            }
        }

        private void TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                returningNothingFunction();
            }
            catch (InvalidLoginViewException invalidStudentViewException)
            {
                throw CreateAndLogValidationException(invalidStudentViewException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private LoginViewValidationException CreateAndLogValidationException(Exception exception)
        {
            var loginViewValidationException = new LoginViewValidationException(exception, exception.Data);
            this.loggingBroker.LogError(loginViewValidationException);

            return loginViewValidationException;
        }

        private LoginViewDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var loginViewDependencyException = new LoginViewDependencyException(exception);
            this.loggingBroker.LogError(loginViewDependencyException);

            return loginViewDependencyException;
        }

        private LoginViewServiceException CreateAndLogServiceException(Exception exception)
        {
            var loginViewServiceException = new LoginViewServiceException(exception);
            this.loggingBroker.LogError(loginViewServiceException);

            return loginViewServiceException;
        }
    }
}
