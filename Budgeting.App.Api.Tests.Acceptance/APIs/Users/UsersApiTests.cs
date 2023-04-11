using Budgeting.App.Api.Extensions;
using Budgeting.App.Api.Tests.Acceptance.Brokers;
using Budgeting.App.Api.Tests.Acceptance.Models.Users;
using System;
using Tynamix.ObjectFiller;
using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.APIs.Users
{
    [Collection(nameof(ApiTestCollection))]
    public partial class UsersApiTests
    {
        private readonly BudgetingAppApiBroker budgetingAppApiBroker;

        public UsersApiTests(BudgetingAppApiBroker budgetingAppApiBroker)
        {
            this.budgetingAppApiBroker = budgetingAppApiBroker;
        }

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();

        private static User CreateRandomUser() =>
            new User
            {
                Id = Guid.NewGuid(),
                Email = "travis@mail.com",
                FirstName = "Travis",
                LastName = "Kongo",
                CreatedDate = DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)),
                UpdatedDate = DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)).AddHours(1),
            };

        private static User UpdateUserRandom(User user)
        {
            user.FirstName = "Maylo";
            user.LastName = "North";

            return user;
        }
    }
}
