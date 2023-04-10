using Budgeting.App.Api.Models.Users;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldRemoveApplicationUserById()
        {
            //given
            User randomUser = CreateUser();
            Guid userId = randomUser.Id;
            User storageUser = randomUser;
            User expectedUser = storageUser.DeepClone();

            this.userManagerBrokerMock.Setup(broker =>
               broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(storageUser);

            this.userManagerBrokerMock.Setup(broker =>
               broker.DeleteUserAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            //when
            User actualUser =
                 await this.userService.RemoveUserByIdAsync(userId);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagerBrokerMock.Verify(broker =>
               broker.SelectUserByIdAsync(It.IsAny<Guid>()),
               Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
               broker.DeleteUserAsync(It.IsAny<User>()),
               Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
