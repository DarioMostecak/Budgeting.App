using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityRequests.Exceptions;
using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAuthenticateWhenRequestIsNullAndLogItAsync()
        {
            //given
            IdentityRequest nullRequest = null;

            var nullIdentityRequestException =
                new NullIdentityRequestException();

            var expectedIdentityRequestValidationException =
                new IdentityRequestValidationException(
                    innerException: nullIdentityRequestException,
                    data: nullIdentityRequestException.Data);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(nullRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestValidationException>(() =>
                authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestValidationException))),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidIdentityRequestData))]
        public async Task ShouldThrowValidationExceptionOnAuthenticationIfIdentityRequestIsInvalidAndLogItAsync(
            string invalidEmail,
            string invalidPassword)
        {
            //given
            var invalidIdentityRequest = new IdentityRequest
            {
                Email = invalidEmail,
                Password = invalidPassword
            };

            var invalidIdentityRequestException =
                new InvalidIdentityRequestException();

            invalidIdentityRequestException.AddData(
                key: nameof(IdentityRequest.Email),
                values: "Email can't be white space or null and must be type of email format.");

            invalidIdentityRequestException.AddData(
                key: nameof(IdentityRequest.Password),
                values: "Password is required.");

            var expectedIdentityRequestValidationException =
                new IdentityRequestValidationException(
                    invalidIdentityRequestException,
                    invalidIdentityRequestException.Data);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(invalidIdentityRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestValidationException>(() =>
                authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedIdentityRequestValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAuthenticationIfIdentityIsNullInvalidAndLogItAsync()
        {
            //given
            User nullUser = null;
            IdentityRequest identityRequest = CreateRandomIdentityRequest();

            var failAuthenticationIdentityRequestException =
                new FailAuthenticationIdentityRequestException(identityRequest.Email);

            var expectedIdentityRequestValidationException =
                new IdentityRequestValidationException(
                    innerException: failAuthenticationIdentityRequestException,
                    data: failAuthenticationIdentityRequestException.Data);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByEmailAsync(It.IsAny<string>()))
                   .ReturnsAsync(nullUser);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(identityRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestValidationException>(() =>
                authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestValidationException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                 broker.SelectUserByEmailAsync(It.IsAny<string>()),
                   Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAuthenticationIfPasswordIsInvalidAndLogItAsync()
        {
            //given
            IdentityRequest randomIdentityRequest =
                CreateRandomIdentityRequest();

            User randomUser = CreateRandomUser();

            var failAuthenticationIdentityRequestException =
                new FailAuthenticationIdentityRequestException(randomIdentityRequest.Password);

            var expectedIdentityRequestValidationException =
                new IdentityRequestValidationException(
                    innerException: failAuthenticationIdentityRequestException,
                    data: failAuthenticationIdentityRequestException.Data);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(randomUser);

            this.userManagerBrokerMock.Setup(broker =>
                broker.ConfirmUserByPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                   .ReturnsAsync(false);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(randomIdentityRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestValidationException>(() =>
                authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestValidationException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                 broker.SelectUserByEmailAsync(It.IsAny<string>()),
                   Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                 broker.ConfirmUserByPasswordAsync(
                     It.IsAny<User>(), It.IsAny<string>()),
                       Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();

        }
    }
}
