using Budgeting.App.Api.Models.Categories;
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
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category afterUpdateCategory = inputCategory;
            Category expectedCategory = afterUpdateCategory;
            Category beforeUpdateStorageCategory = randomCategory.DeepClone();
            inputCategory.TimeModify = randomDate;
            Guid categoryId = inputCategory.CategoryId;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(categoryId))
                .ReturnsAsync(beforeUpdateStorageCategory);

            this.storageBrokerMock.Setup(broker =>
               broker.UpdateCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(afterUpdateCategory);

            //when
            Category actualCategory = await this.categoryService.ModifyCategoryAsync(inputCategory);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

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
