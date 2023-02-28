using Budgeting.App.Api.Models.Categories;
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
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category storageCategory = randomCategory;
            Category expectedCategory = storageCategory.DeepClone();

            this.storageBrokerMock.Setup(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(storageCategory);

            //When
            Category actualCategory = await this.categoryService.AddCategoryAsync(inputCategory);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
               Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
