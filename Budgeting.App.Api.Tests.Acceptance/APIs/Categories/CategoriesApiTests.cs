using Budgeting.App.Api.Extensions;
using Budgeting.App.Api.Tests.Acceptance.Brokers;
using Budgeting.App.Api.Tests.Acceptance.Models.Categories;
using System;
using System.Collections.Generic;
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

        private static IEnumerable<Category> GetRandomCategories() =>
            CreateRandomCategoryFiller().Create(GetRandomNumber());

        private static Category CreateRandomCategory() =>
            CreateRandomCategoryFiller().Create();

        private static string GetRandomString(int wordCount, int wordMinLength, int wordMaxLength)
            => new MnemonicString(wordCount, wordMinLength, wordMaxLength).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static Filler<Category> CreateRandomCategoryFiller()
        {
            var filler = new Filler<Category>();
            Guid categoryId = Guid.NewGuid();

            filler.Setup()
                .OnProperty(category => category.CategoryId).Use(categoryId)
                .OnProperty(category => category.Title).Use(GetRandomString(1, 4, 10))
                .OnProperty(category => category.Icon).Use(GetRandomString(1, 4, 10))
                .OnProperty(category => category.Type).Use("Expense")
                .OnProperty(category => category.TimeCreated).Use(DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)))
                .OnProperty(category => category.TimeModify).Use(DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)).AddHours(1));

            return filler;
        }

        private Category UpdateCategoryRandom(Category category)
        {
            category.Title = new MnemonicString(1, 4, 10).GetValue();
            category.TimeModify = DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)).AddHours(2);

            return category;
        }
    }
}
