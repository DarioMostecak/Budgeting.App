using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.Categories.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
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
                new CategoryValidationException(
                    nullCategoryException,
                    nullCategoryException.Data);

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

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(
                    It.IsAny<Category>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataCategory))]
        public async Task ShouldThrowValidationExceptionOnModifyIfCategoryIsInvalidAndLogItAsync(
           Guid invalidId,
           string invalidTitle,
           string invalidType,
           DateTime invalidDateCreated,
           DateTime invalidDateModify)
        {
            //given
            var invalidCategory = new Category
            {
                CategoryId = invalidId,
                Title = invalidTitle,
                Type = invalidType,
                TimeCreated = invalidDateCreated,
                TimeModify = invalidDateModify
            };

            var invalidCategoryException = new InvalidCategoryException();

            invalidCategoryException.AddData(
                key: nameof(Category.CategoryId),
                values: "Id isn't valid.");

            invalidCategoryException.AddData(
                key: nameof(Category.Title),
                values: "Value can't be null, white space or empty.");

            invalidCategoryException.AddData(
                key: nameof(Category.Type),
                values: "Value can't be null, white space or empty.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeCreated),
                values: "Date is required.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeModify),
                values: new string[] { "Date is required.", $"Date is the same as {nameof(Category.TimeModify)}" });

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException, invalidCategoryException.Data);

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

            this.apiBrokerMock.Verify(broker =>
                broker.PutCategoryAsync(
                    It.IsAny<Category>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
