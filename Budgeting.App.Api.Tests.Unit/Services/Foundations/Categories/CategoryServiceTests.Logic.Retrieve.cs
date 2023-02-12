using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCategories()
        {
            //given
            DateTime randomDate = GetRandomDateTime();
            IQueryable<CategoryDto> randomCategories = CreateRandomCategoryDtos(randomDate);
            IQueryable<Category> storageCategories = ProjectToCategory(randomCategories);
            IQueryable<CategoryDto> expectedCategoryDtos = randomCategories;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllCategories())
                .Returns(storageCategories);

            //when
            IQueryable<CategoryDto> actualCategoryDtos = this.categoryService.RetrieveAllCategories();

            //then
            actualCategoryDtos.Should().BeEquivalentTo(expectedCategoryDtos);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectAllCategories(),
                  Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
