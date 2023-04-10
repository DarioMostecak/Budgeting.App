using Budgeting.App.Api.Models.Categories;
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
            IQueryable<Category> randomCategories = CreateRandomCategories(randomDate);
            IQueryable<Category> storageCategories = randomCategories;
            IQueryable<Category> expectedCategories = storageCategories;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectAllCategories())
                .Returns(storageCategories);

            //when
            IQueryable<Category> actualCategories =
                this.categoryService.RetrieveAllCategories();

            //then
            actualCategories.Should().BeEquivalentTo(expectedCategories);

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
