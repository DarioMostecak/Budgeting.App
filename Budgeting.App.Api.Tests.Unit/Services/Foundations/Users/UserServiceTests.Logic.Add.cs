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
        public async Task ShoudAddApplicationUserAsync()
        {
            //given
            User nullUser = null;
            User randomUser = CreateUser();
            string userPassword = GetRandomPassword();

            User inputUser = randomUser;
            User storageUser = randomUser;
            User expectedUser = storageUser.DeepClone();

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByEmailAsync(It.IsAny<string>()))
                 .ReturnsAsync(nullUser);

            this.userManagerBrokerMock.Setup(manager =>
                manager.InsertUserAsync(inputUser, It.IsAny<string>()))
                 .ReturnsAsync(IdentityResult.Success);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(storageUser);

            //when
            User actualUser =
                await this.userService.AddUserAsync(inputUser, userPassword);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagerBrokerMock.Verify(manager =>
                manager.InsertUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()),
                     Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                 manager.SelectUserByEmailAsync(
                     It.IsAny<string>()),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                 manager.SelectUserByIdAsync(
                     It.IsAny<Guid>()),
                      Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
