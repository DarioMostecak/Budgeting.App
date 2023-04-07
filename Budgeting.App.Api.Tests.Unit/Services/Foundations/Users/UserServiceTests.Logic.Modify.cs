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
        public async Task ShouldModifyUserAsync()
        {
            //given
            DateTime randomDate = GetRandomDateTime();
            User randomUser = CreateUser();
            User inputUser = randomUser;
            User afterUpadteUser = inputUser;
            User expectedUser = afterUpadteUser;
            User beforeUpdateStorageUser = randomUser.DeepClone();
            inputUser.UpdatedDate = randomDate;
            Guid userId = inputUser.Id;

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(userId))
                   .ReturnsAsync(beforeUpdateStorageUser);

            this.userManagerBrokerMock.Setup(manager =>
                manager.UpdateUserAsync(It.IsAny<User>()))
                  .ReturnsAsync(IdentityResult.Success);

            //when
            User actualUser =
                await this.userService.ModifyUserAsync(inputUser);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once());

            this.userManagerBrokerMock.Verify(broker =>
               broker.UpdateUserAsync(It.IsAny<User>()),
                 Times.Once());

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
