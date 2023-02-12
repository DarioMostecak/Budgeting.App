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
        public async Task ShouldRetreiveCategoryById()
        {
            //given
            CategoryDto randomCategoryDto = CreateRandomCategoryDto();
            Category storageCategory = randomCategoryDto;
            CategoryDto expectedCategoryDto = randomCategoryDto.DeepClone();
            Guid categoryId = randomCategoryDto.CategoryId;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(categoryId))
                .ReturnsAsync(storageCategory);

            //when
            CategoryDto actualCategoryDto = await this.categoryService.RetriveCategoryByIdAsync(categoryId);

            //then
            actualCategoryDto.Should().BeEquivalentTo(expectedCategoryDto);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                 Times.Once());


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
