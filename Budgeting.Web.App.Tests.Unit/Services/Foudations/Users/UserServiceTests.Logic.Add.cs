using Budgeting.Web.App.Models.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task SouldAddUserAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            User inputUser = someUser;
            User returnedUser = inputUser;
            User expectedUser = someUser.DeepClone();

            this.apiBrokerMock.Setup(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ReturnsAsync(returnedUser);

            //when
            User actualUser =
                await this.userService.AddUserAsync(inputUser, password);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.apiBrokerMock.Verify(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()),
                      Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
