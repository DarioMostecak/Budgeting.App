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
        public async Task ShouldThrowValidationExceptionOnAddWhenCategoryIsNullAndLogItAsync()
        {
            //given
            Category invalidCategory = null;
            var nullCategoryException = new NullCategoryException();

            var expectedCategoryException =
                new CategoryValidationException(
                    nullCategoryException,
                    nullCategoryException.Data);

            //when
            ValueTask<Category> addCategoryTask =
                 this.categoryService.AddCategoryAsync(invalidCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
               addCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryException))),
                      Times.Once());

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataCategory))]
        public async Task ShouldThrowValidationExceptionOnAddIfCategoryIsInvalidAndLogItAsync(
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
                values: "Must be between 2 and 19 characters long and can't be null or white space.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeCreated),
                values: "Date is required.");

            invalidCategoryException.AddData(
                key: nameof(Category.TimeModify),
                values: "Date is required.");

            var expectedCategoryValidationException =
                new CategoryValidationException(invalidCategoryException, invalidCategoryException.Data);

            //when
            ValueTask<Category> addStudentTask =
                this.categoryService.AddCategoryAsync(invalidCategory);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
               addStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameValidationExceptionAs(
                   expectedCategoryValidationException))),
                   Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.InsertCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
