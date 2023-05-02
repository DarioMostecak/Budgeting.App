using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.UserViews;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views.UserViews
{
    public partial class UserViewServiceTests
    {
        [Fact]
        public async Task ShouldAddUserViewAsync()
        {
            //given
            UserView someUserView = CreateUserView();
            UserView inputUserView = someUserView;
            UserView expectedUserView = someUserView.DeepClone();

            this.userServiceMock.Setup(broker =>
                broker.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ReturnsAsync(It.IsAny<User>());

            //when
            UserView actualUserView =
                await this.userViewService.AddUserViewAsync(inputUserView);

            //then
            actualUserView.Should().BeEquivalentTo(expectedUserView);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()),
                      Times.Once);

            this.dateTimeBroker.Verify(broker =>
                 broker.GetCurrentDateTime(),
                   Times.Once);

            this.uniqueIDGeneratorBroker.Verify(broker =>
                 broker.GenerateUniqueID(),
                   Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBroker.VerifyNoOtherCalls();
        }
    }
}
