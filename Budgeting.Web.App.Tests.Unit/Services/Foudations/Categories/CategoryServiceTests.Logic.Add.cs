using Budgeting.Web.App.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShoudAddCategoryAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category savedCategory = randomCategory;
            Category expectedCategory = savedCategory.DeepClone();

            this.apiBrokerMock.Setup(broker =>
               broker.PostCategoryAsync(It.IsAny<Category>()))
                .ReturnsAsync(savedCategory);

            //When
            Category actualCategory = await this.categoryService.AddCategoryAsync(inputCategory);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.apiBrokerMock.Verify(broker =>
               broker.PostCategoryAsync(It.IsAny<Category>()),
               Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
