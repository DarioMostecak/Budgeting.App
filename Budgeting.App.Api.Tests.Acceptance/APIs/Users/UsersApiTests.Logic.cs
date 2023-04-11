using Budgeting.App.Api.Tests.Acceptance.Models.Users;
using FluentAssertions;
using Force.DeepCloner;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.APIs.Users
{
    public partial class UsersApiTests
    {
        [Fact]
        public async Task ShouldPostUserAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            User expectedUser = randomUser.DeepClone();
            string randomPassword = GetRandomPassword();

            //when
            await this.budgetingAppApiBroker.PostUserAsync(inputUser, randomPassword);
            await this.budgetingAppApiBroker.AddAuthenticationHeaderAsync(randomUser, randomPassword);

            User actualUser =
                await this.budgetingAppApiBroker.GetUserByIdAsync(inputUser.Id);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);
            await this.budgetingAppApiBroker.DeleteUserAsync(inputUser.Id);
            await this.budgetingAppApiBroker.RemoveAuthenticationHeaderAsync();
        }

        [Fact]
        public async Task ShouldPutUserAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            User modifyUser = UpdateUserRandom(randomUser);
            string randomPassword = GetRandomPassword();

            await this.budgetingAppApiBroker.PostUserAsync(randomUser, randomPassword);
            await this.budgetingAppApiBroker.AddAuthenticationHeaderAsync(randomUser, randomPassword);

            //when
            await this.budgetingAppApiBroker.PutUserAsync(modifyUser);

            User actualUser =
                await this.budgetingAppApiBroker.GetUserByIdAsync(randomUser.Id);

            //then
            actualUser.Should().BeEquivalentTo(modifyUser);
            await this.budgetingAppApiBroker.DeleteUserAsync(actualUser.Id);
            await this.budgetingAppApiBroker.RemoveAuthenticationHeaderAsync();
        }

        [Fact]
        public async Task ShouldGetUserByIdAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            User expectedUser = randomUser.DeepClone();
            string randomPassword = GetRandomPassword();

            await this.budgetingAppApiBroker.PostUserAsync(randomUser, randomPassword);
            await this.budgetingAppApiBroker.AddAuthenticationHeaderAsync(randomUser, randomPassword);

            //when
            User actualUser =
                await this.budgetingAppApiBroker.GetUserByIdAsync(inputUser.Id);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);
            await this.budgetingAppApiBroker.DeleteUserAsync(actualUser.Id);
            await this.budgetingAppApiBroker.RemoveAuthenticationHeaderAsync();
        }

        [Fact]
        public async Task ShouldDeleteUserAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputCategory = randomUser;
            User expectedUser = randomUser.DeepClone();
            string randomPassword = GetRandomPassword();

            await this.budgetingAppApiBroker.PostUserAsync(randomUser, randomPassword);
            await this.budgetingAppApiBroker.AddAuthenticationHeaderAsync(randomUser, randomPassword);

            //when
            User actualUser =
                await this.budgetingAppApiBroker.DeleteUserAsync(inputCategory.Id);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);
            await this.budgetingAppApiBroker.RemoveAuthenticationHeaderAsync();
        }
    }
}
