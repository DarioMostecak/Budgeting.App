using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetriveIfIdIsEmptyAndLogItAsync()
        {
            //when
            Guid invalidId = Guid.Empty;

            var invalidCategoryException =
                new InvalidCategoryException(nameof(Category.CategoryId), invalidId);

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            //then
            ValueTask<Category> retriveCategoryTask =
                this.categoryService.RetriveCategoryByIdAsync(invalidId);

            //when
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                retriveCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCategoryValidationException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoriesByIdAsync(
                    It.IsAny<Guid>()),
                      Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetriveIfNotFoundAndLogItAsync()
        {
            Guid randomGuid = Guid.NewGuid();
            Category noStorageCategory = null;

            var notFoundCategoryException =
                new NotFoundCategoryException(randomGuid);

            var expectedValidationException =
                new CategoryValidationException(notFoundCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoriesByIdAsync(randomGuid))
                .ReturnsAsync(noStorageCategory);

            //when
            ValueTask<Category> retriveCategoryTask =
                this.categoryService.RetriveCategoryByIdAsync(randomGuid);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                retriveCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationException))),
                     Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoriesByIdAsync(
                    It.IsAny<Guid>()),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
