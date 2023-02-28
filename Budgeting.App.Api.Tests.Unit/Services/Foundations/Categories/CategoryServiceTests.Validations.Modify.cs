using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
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
        public async Task ShouldThrowValidationExceptionOnModifyWhenCategoryIsNullAndLogItAsync()
        {
            //given
            Category invalidCategory = null;
            var nullCategoryException = new NullCategoryException();

            var expectedCategoryValidationException =
                new CategoryValidationException(nullCategoryException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(invalidCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

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

        [Theory]
        [MemberData(nameof(InvalidDataCategory))]
        public async Task ShouldThrowValidationExceptionOnModifyIfCategoryIsInvalidAndLogItAsync(
            Guid invalidId,
            string invalidTitle,
            DateTime invalidDateCreated,
            DateTime invalidDateModify)
        {
            //given
            var invalidCategory = new Category
            {
                CategoryId = invalidId,
                Title = invalidTitle,
                TimeCreated = invalidDateCreated,
                TimeModify = invalidDateModify
            };

            var invalidCategoryException = new InvalidCategoryException();

            invalidCategoryException.AddData(
                key: nameof(Category.CategoryId),
                values: "Id isn't valid.");

            invalidCategoryException.AddData(
                key: nameof(Category.Title),
                values: "Category title isn't valid. Must be between 2 and 19 characters long.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeCreated),
                values: "Date is required.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeModify),
                values: new string[] { "Date is required.", $"Date is the same as {nameof(Category.TimeModify)}" });

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(invalidCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
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
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCategoryIsNullAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category nonExistentCategory = randomCategory;
            Category noCategory = null;

            var notFoundCategoryException =
                new NotFoundCategoryException(nonExistentCategory.CategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(notFoundCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoriesByIdAsync(nonExistentCategory.CategoryId))
                     .ReturnsAsync(noCategory);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(randomCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCategoriesByIdAsync(
                    It.IsAny<Guid>()),
                      Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCategoryValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIdAndCratedDatesAreNotSameAndModifyDateIsSameAndLogItAsync()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            DateTime sameDate = randomCategory.TimeModify;
            DateTime differentDate = GetRandomDateTime();
            Guid differentId = Guid.NewGuid();
            Category invalidCategory = randomCategory;
            Category storageCategory = randomCategory.DeepClone();
            storageCategory.CategoryId = differentId;
            storageCategory.TimeModify = sameDate;
            storageCategory.TimeCreated = differentDate;

            var invalidCategoryException = new InvalidCategoryException();

            invalidCategoryException.AddData(
                key: nameof(Category.CategoryId),
                values: $"Id is not the same as {nameof(Category.CategoryId)}");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeCreated),
                values: $"Date is not the same as {nameof(Category.TimeCreated)}");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeModify),
                values: $"Date is the same as {nameof(Category.TimeModify)}");

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCategoriesByIdAsync(invalidCategory.CategoryId))
                 .ReturnsAsync(storageCategory);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(invalidCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCategoryValidationException))),
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
