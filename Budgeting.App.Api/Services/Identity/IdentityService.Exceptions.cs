using Budgeting.App.Api.Models.IdentityRequests.Exceptions;
using Budgeting.App.Api.Models.IdentityResponses;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Budgeting.App.Api.Services.Identity
{
    public partial class IdentityService
    {
        private delegate ValueTask<IdentityResponse> ReturnigIdentityResponseFunctions();

        private async ValueTask<IdentityResponse> TryCatch(
            ReturnigIdentityResponseFunctions returnigIdentityResponeFunctions)
        {
            try
            {
                return await returnigIdentityResponeFunctions();
            }
            catch (NullIdentityRequestException nullIdentityRequestException)
            {
                throw CreateAndLogValidationException(nullIdentityRequestException);
            }
            catch (InvalidIdentityRequestException invalidIdentityException)
            {
                throw CreateAndLogValidationException(invalidIdentityException);
            }
            catch (FailAuthenticationIdentityRequestException failAuthenticationIdentityRequestException)
            {
                throw CreateAndLogValidationException(failAuthenticationIdentityRequestException);
            }
            catch (MongoException mongoException)
            {
                var failIdentityRequestserviceException =
                    new FailIdentityRequestServiceException(mongoException);

                throw CreateAndLogDependencyException(failIdentityRequestserviceException);
            }
            catch (SecurityTokenEncryptionFailedException securityTokenEncryptionFailedException)
            {
                var failIdentityRequestServiceException =
                    new FailIdentityRequestServiceException(securityTokenEncryptionFailedException);

                throw CreateAndLogServiceException(failIdentityRequestServiceException);
            }
            catch (Exception exception)
            {
                var failIdentityRequestserviceException =
                    new FailIdentityRequestServiceException(exception);

                throw CreateAndLogServiceException(failIdentityRequestserviceException);
            }
        }

        private IdentityRequestValidationException CreateAndLogValidationException(Exception exception)
        {
            var identityRequestValidationException =
                new IdentityRequestValidationException(exception, exception.Data);

            this.loggingBroker.LogError(identityRequestValidationException);

            return identityRequestValidationException;
        }

        private IdentityRequestDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var identityRequestDependencyException =
                new IdentityRequestDependencyException(exception);

            this.loggingBroker.LogError(identityRequestDependencyException);

            return identityRequestDependencyException;
        }

        private IdentityRequestServiceException CreateAndLogServiceException(Exception exception)
        {
            var identityRequestServiceException =
                new IdentityRequestServiceException(exception);

            this.loggingBroker.LogError(identityRequestServiceException);

            return identityRequestServiceException;
        }
    }
}
