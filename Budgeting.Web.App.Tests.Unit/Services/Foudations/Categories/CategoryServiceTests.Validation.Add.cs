// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.Categories.Exceptions;
using Moq;
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
    }
}
