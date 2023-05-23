using Budgeting.Web.App.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {

        [Fact]
        public async Task ShouldRemoveCategoryById()
        {
            //given
            Category randomCateogry = CreateRandomCategory();
            Category deletedCategory = randomCateogry;
            Category expectedCategory = deletedCategory.DeepClone();
            Guid categoryId = randomCateogry.CategoryId;


            this.apiBrokerMock.Setup(broker =>
                broker.DeleteCategoryAsync(categoryId.ToString()))
                        .ReturnsAsync(deletedCategory);

            //when
            Category actualCategory =
                await this.categoryService.RemoveCategoryByIdAsync(categoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.apiBrokerMock.Verify(broker =>
                broker.DeleteCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
