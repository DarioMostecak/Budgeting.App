using Budgeting.App.Api.Tests.Acceptance.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.APIs.Categories
{
    public partial class CategoriesApiTests
    {
        [Fact]
        public async Task ShouldPostCategoryAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category expectedCategory = randomCategory.DeepClone();

            await this.budgetingAppApiBroker.PostCategoryAsync(inputCategory);

            //when
            Category actualCategory =
                await this.budgetingAppApiBroker.GetCategoryByIdAsync(inputCategory.CategoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);
            await this.budgetingAppApiBroker.DeleteCategoryAsync(actualCategory.CategoryId);
        }

        [Fact]
        public async Task ShouldPutCategoryAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category modifyCategory = UpdateCategoryRandom(randomCategory);

            await this.budgetingAppApiBroker.PostCategoryAsync(randomCategory);

            //when
            await this.budgetingAppApiBroker.PutCategoryAsync(modifyCategory);

            Category actualCategory =
                await this.budgetingAppApiBroker.GetCategoryByIdAsync(randomCategory.CategoryId);

            //then
            actualCategory.Should().BeEquivalentTo(modifyCategory);
            await this.budgetingAppApiBroker.DeleteCategoryAsync(actualCategory.CategoryId);
        }

        [Fact]
        public async Task ShoudGetAllCategoriesAsync()
        {
            //given
            IEnumerable<Category> randomCategories = GetRandomCategories();
            IEnumerable<Category> inputCategories = randomCategories;

            foreach (var category in inputCategories)
            {
                await this.budgetingAppApiBroker.PostCategoryAsync(category);
            }

            List<Category> expectedCategories = inputCategories.ToList();

            //when
            List<Category> actualCategories = await this.budgetingAppApiBroker.GetAllCategoriesAsync();

            //then
            foreach (var expectedCategory in expectedCategories)
            {
                Category actualCategory = actualCategories.Single(category => category.CategoryId == expectedCategory.CategoryId);
                actualCategory.Should().BeEquivalentTo(expectedCategory);
                await this.budgetingAppApiBroker.DeleteCategoryAsync(actualCategory.CategoryId);
            }
        }

        [Fact]
        public async Task ShouldDeleteCategoryAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category expectedCategory = inputCategory.DeepClone();

            await this.budgetingAppApiBroker.PostCategoryAsync(randomCategory);

            //when
            Category actualCategory =
                await this.budgetingAppApiBroker.DeleteCategoryAsync(inputCategory.CategoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

        }

        [Fact]
        public async Task ShouldGetCategoryByIdAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category expectedCategory = inputCategory.DeepClone();

            await this.budgetingAppApiBroker.PostCategoryAsync(randomCategory);

            //when
            Category actualCategory =
                await this.budgetingAppApiBroker.GetCategoryByIdAsync(inputCategory.CategoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);
            await this.budgetingAppApiBroker.DeleteUserAsync(actualCategory.CategoryId);
        }
    }
}
