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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsEmptyAndLogItAsync()
        {
            //given
            Guid invalidId = Guid.Empty;

            var invalidCategoryException =
                new InvalidCategoryException(
                    nameof(Category.CategoryId),
                    invalidId);

            var expectedCategoryValidationException =
                new CategoryValidationException(
                    invalidCategoryException,
                    invalidCategoryException.Data);

            //when
            ValueTask<Category> removeCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(invalidId);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                removeCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCategoryValidationException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfStorageCategoryIsNullAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category noCategory = null;

            var notFoundCategoryException =
                new NotFoundCategoryException(inputCategory.CategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(
                    notFoundCategoryException,
                    notFoundCategoryException.Data);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoriesByIdAsync(inputCategory.CategoryId))
                  .ReturnsAsync(noCategory);

            //when
            ValueTask<Category> removeCategoryTask =
                this.categoryService.RemoveCategoryByIdAsync(inputCategory.CategoryId);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                removeCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCategoryValidationException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

