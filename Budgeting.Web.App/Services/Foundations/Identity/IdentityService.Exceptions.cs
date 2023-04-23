using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.AuthenticationResults.Exceptions;
using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Services.Foundations
{
    public partial class IdentityService
    {
        private delegate ValueTask<AuthenticationResult> AuthenticateReturnigFunction();

        private async ValueTask<AuthenticationResult> TryCatch(
            AuthenticateReturnigFunction authenticateReturnigFunction)
        {
            try
            {
                return await authenticateReturnigFunction();
            }
            catch (NullAuthenticationRequestException nullAuthenticationRequestException)
            {
                throw CreateAndLogValidationException(nullAuthenticationRequestException);
            }
            catch (InvalidAuthenticationRequestException invalidAuthenticationRequestException)
            {
                throw CreateAndLogValidationException(invalidAuthenticationRequestException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizeException)
            {
                throw CreateAndLogUnauthorizedException(httpResponseUnauthorizeException);
            }
            catch (NullAuthenticationResultException nullAuthenticationResultException)
            {
                var failedAuthenticationRequestDependencyException =
                    new FailedAuthenticationRequestDependencyException(nullAuthenticationResultException);

                throw CreateAndLogDependencyException(failedAuthenticationRequestDependencyException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var failedAuthenticationRequestDependencyException =
                    new FailedAuthenticationRequestDependencyException(httpResponseBadRequestException);

                throw CreateAndLogDependencyException(failedAuthenticationRequestDependencyException);
            }
            catch (HttpResponseInternalServerErrorException httpResponseInternalServerErrorException)
            {
                var failedAuthenticationRequestDependencyException =
                    new FailedAuthenticationRequestDependencyException(httpResponseInternalServerErrorException);

                throw CreateAndLogDependencyException(failedAuthenticationRequestDependencyException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedAuthenticationRequestDependencyException =
                    new FailedAuthenticationRequestDependencyException(httpRequestException);

                throw CreateAndLogDependencyException(failedAuthenticationRequestDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedAuthenticationRequestDependencyException =
                    new FailedAuthenticationRequestDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedAuthenticationRequestDependencyException);
            }
            catch (Exception exception)
            {
                var failedAuthenticationRequestServiceException =
                    new FailedAuthenticationRequestServiceException(exception);

                throw CreateAndLogServiceException(failedAuthenticationRequestServiceException);
            }

        }

        private AuthenticationRequestValidationException CreateAndLogValidationException(Exception exception)
        {
            var authenticationRequestValidationException = new AuthenticationRequestValidationException(exception, exception.Data);
            this.loggingBroker.LogError(authenticationRequestValidationException);

            return authenticationRequestValidationException;
        }

        private AuthenticationRequestDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var authenticationRequestDependencyException = new AuthenticationRequestDependencyException(exception);
            this.loggingBroker.LogError(authenticationRequestDependencyException);

            return authenticationRequestDependencyException;
        }

        private AuthenticationRequestServiceException CreateAndLogServiceException(Exception exception)
        {
            var authenticationRequestServiceException = new AuthenticationRequestServiceException(exception);
            this.loggingBroker.LogError(authenticationRequestServiceException);

            return authenticationRequestServiceException;
        }

        private AuthenticationRequestUnauthorizedException CreateAndLogUnauthorizedException(Exception exception)
        {
            var authenticationRequestUnauthorizedException = new AuthenticationRequestUnauthorizedException(exception);
            this.loggingBroker.LogError(authenticationRequestUnauthorizedException);

            return authenticationRequestUnauthorizedException;
        }
    }
}
