using Budgeting.App.Api.Models.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveApplicationUserById()
        {
            //given
            User randomUser = CreateUser();
            User storageUser = randomUser;
            User expectedUser = storageUser.DeepClone();
            Guid userId = randomUser.Id;

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(userId))
                  .ReturnsAsync(storageUser);

            //when
            User actualUser =
                await this.userService.RetrieveUserByIdAsync(userId);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
