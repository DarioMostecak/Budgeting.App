using Budgeting.App.Api.Tests.Acceptance.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
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

            //when
            await this.budgetingAppApiBroker.PostCategoryAsync(inputCategory);

            Category actualCategory =
                await this.budgetingAppApiBroker.GetCategoryByIdAsync(inputCategory.CategoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);
            await this.budgetingAppApiBroker.DeleteCategoryAsync(actualCategory.CategoryId);
        }
    }
}
