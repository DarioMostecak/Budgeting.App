using Budgeting.App.Api.Tests.Acceptance.Brokers;
using Budgeting.App.Api.Tests.Acceptance.Models.Categories;
using System;
using Tynamix.ObjectFiller;
using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.APIs.Categories
{
    [Collection(nameof(ApiTestCollection))]
    public partial class CategoriesApiTests
    {
        private readonly BudgetingAppApiBroker budgetingAppApiBroker;

        public CategoriesApiTests(BudgetingAppApiBroker budgetingAppApiBroker)
        {
            this.budgetingAppApiBroker = budgetingAppApiBroker;
        }

        private static Category CreateRandomCategory() =>
            CreateRandomCategoryFiller(dates: DateTime.UtcNow).Create();

        private static Filler<Category> CreateRandomCategoryFiller(DateTime dates)
        {
            var filler = new Filler<Category>();
            Guid categoryId = Guid.NewGuid();

            filler.Setup()
                .OnProperty(category => category.CategoryId).Use(categoryId)
                .OnProperty(category => category.Title).Use("Lunch")
                .OnProperty(category => category.Icon).Use("ffff")
                .OnProperty(category => category.Type).Use("Expense")
                .OnProperty(category => category.TimeCreated).Use(dates)
                .OnProperty(category => category.TimeModify).Use(dates.AddDays(10));

            return filler;
        }
    }
}
