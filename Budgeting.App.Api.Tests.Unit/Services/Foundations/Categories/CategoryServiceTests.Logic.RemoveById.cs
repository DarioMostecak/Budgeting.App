using Budgeting.App.Api.Models.Categories;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCategoryById()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Guid categoryId = randomCategory.CategoryId;
            Category storageCategory = randomCategory;
            Category expectedCategory = storageCategory;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(storageCategory);

            this.storageBrokerMock.Setup(broker =>
               broker.DeleteCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(expectedCategory);

            //when
            Category actualCategory =
                await this.categoryService.RemoveCategoryByIdAsync(categoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
               Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.DeleteCategoryAsync(It.IsAny<Category>()),
               Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
