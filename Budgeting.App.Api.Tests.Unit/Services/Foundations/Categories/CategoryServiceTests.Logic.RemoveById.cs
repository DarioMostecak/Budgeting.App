using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;
using FluentAssertions;
using Force.DeepCloner;
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
            CategoryDto randomCategoryDto = CreateRandomCategoryDto();
            Guid categoryId = randomCategoryDto.CategoryId;
            Category storageCategory = randomCategoryDto;
            Category expectedCategory = storageCategory;
            CategoryDto expectedCategoryDto = (CategoryDto)storageCategory.DeepClone();

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(storageCategory);

            this.storageBrokerMock.Setup(broker =>
               broker.DeleteCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(expectedCategory);

            //when
            CategoryDto actualCategory = await this.categoryService.RemoveCategoryByIdAsync(categoryId);

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
