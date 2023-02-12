using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShoudCreateCategoryAsync()
        {
            //given
            CategoryDto randomCategory = CreateRandomCategoryDto();
            CategoryDto inputCategoryDto = randomCategory;
            Category inputCategory = inputCategoryDto;
            Category storageCategory = inputCategoryDto;
            CategoryDto expectedCategoryDto = (CategoryDto)storageCategory.DeepClone();

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(storageCategory);

            //When
            CategoryDto actualCategory = await this.categoryService.AddCategoryAsync(inputCategoryDto);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategoryDto);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
               Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
