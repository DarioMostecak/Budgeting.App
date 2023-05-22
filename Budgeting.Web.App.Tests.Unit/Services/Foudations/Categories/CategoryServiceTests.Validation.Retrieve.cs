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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfinvalidCategoryExceptionOccurresAndLogItAsync()
        {
            //given
            Guid invalidCategoryId = Guid.Empty;

            var invalidCategoryException =
                new InvalidCategoryException(
                    parameterName: nameof(Category.CategoryId),
                    parameterValue: invalidCategoryId);

            var expectedCategoryValidationException =
                new CategoryValidationException(
                    innerException: invalidCategoryException,
                    data: invalidCategoryException.Data);

            //when
            ValueTask<Category> retrieveCategoryTask =
                this.categoryService.RetrieveCategoryByIdAsync(invalidCategoryId);

            //then
            await Assert.ThrowsAsync<CategoryValidationException>(() =>
                 retrieveCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCategoryValidationException))),
                      Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
