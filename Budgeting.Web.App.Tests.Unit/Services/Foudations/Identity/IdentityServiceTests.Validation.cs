using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Identity
{
    public partial class IdentityServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAuthenticateIfAuthenticationRequestIsNullAndLogItAsync()
        {
            //given
            AuthenticationRequest invalidRequest = null;

            var nullAuthenticationRequestException =
                new NullAuthenticationRequestException();

            var expectedAuthenticationRequestValidationException =
                new AuthenticationRequestValidationException(
                    nullAuthenticationRequestException,
                    nullAuthenticationRequestException.Data);

            //when
            ValueTask<AuthenticationResult> authenticateIdentityAsTask =
                this.identityService.AuthenticateIdentity(invalidRequest);

            //then
            await Assert.ThrowsAsync<AuthenticationRequestValidationException>(() =>
                authenticateIdentityAsTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthenticationRequestValidationException))),
                       Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataAuthenticationRequest))]
        public async Task ShouldThrowValidationRxceptionOnAuthenticateIfAuthenticationRequestIsInvalidAndLogItAsync(
            string invalidEmail,
            string invalidPassword)
        {
            //given
            var invalidAuthenticationRequest =
                new AuthenticationRequest
                {
                    Email = invalidEmail,
                    Password = invalidPassword
                };

            var invalidAuthenticationRequestException =
                new InvalidAuthenticationRequestException();

            invalidAuthenticationRequestException.AddData(
                key: nameof(AuthenticationRequest.Email),
                values: "Email is required.");

            invalidAuthenticationRequestException.AddData(
                key: nameof(AuthenticationRequest.Password),
                values: "Password is required.");

            var expectedAuthenticationRequestValidationException =
                new AuthenticationRequestValidationException(
                   innerException: invalidAuthenticationRequestException,
                   data: invalidAuthenticationRequestException.Data);

            //when
            ValueTask<AuthenticationResult> authenticateIdentityAsTask =
                this.identityService.AuthenticateIdentity(invalidAuthenticationRequest);

            //then
            await Assert.ThrowsAsync<AuthenticationRequestValidationException>(() =>
                authenticateIdentityAsTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthenticationRequestValidationException))),
                       Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
