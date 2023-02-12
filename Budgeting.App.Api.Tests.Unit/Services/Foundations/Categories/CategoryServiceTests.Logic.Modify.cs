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
        public async Task ShouldModifyCategoryAsync()
        {
            //given
            DateTime randomDate = GetRandomDateTime();
            CategoryDto randomCategoryDto = CreateRandomCategoryDto();
            CategoryDto inputCategoryDto = randomCategoryDto;
            Category inputCategory = inputCategoryDto;
            Category afterUpdateCategory = inputCategory;
            Category beforeUpdateStorageCategory = randomCategoryDto.DeepClone();
            CategoryDto expectedCategoryDto = (CategoryDto)afterUpdateCategory;
            inputCategoryDto.TimeModify = randomDate;
            Guid categoryId = inputCategoryDto.CategoryId;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(categoryId))
                .ReturnsAsync(beforeUpdateStorageCategory);

            this.storageBrokerMock.Setup(broker =>
               broker.UpdateCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(afterUpdateCategory);

            //when
            CategoryDto actualCategory = await this.categoryService.ModifyCategoryAsync(inputCategoryDto);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategoryDto);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(categoryId),
               Times.Once());

            this.storageBrokerMock.Verify(broker =>
               broker.UpdateCategoryAsync(It.IsAny<Category>()),
               Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
