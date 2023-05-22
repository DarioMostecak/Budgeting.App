// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

            this.apiBrokerMock.Verify(broker =>
               broker.PostCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataCategory))]
        public async Task ShouldThrowValidationExceptionOnAddIfCategoryIsInvalidAndLogItAsync(
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
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryValidationException))),
                   Times.Once);

            this.apiBrokerMock.Verify(broker =>
               broker.PostCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
