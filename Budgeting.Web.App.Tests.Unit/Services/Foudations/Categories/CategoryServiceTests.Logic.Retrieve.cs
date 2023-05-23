// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCategoryById()
        {
            //given
            Category randomCateogry = CreateRandomCategory();
            Category retrievedCategory = randomCateogry;
            Category expectedCategory = retrievedCategory.DeepClone();
            Guid categoryId = randomCateogry.CategoryId;


            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoryAsync(categoryId.ToString()))
                        .ReturnsAsync(retrievedCategory);

            //when
            Category actualCategory =
                await this.categoryService.RetrieveCategoryByIdAsync(categoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.apiBrokerMock.Verify(broker =>
                broker.GetCategoryAsync(It.IsAny<string>()),
                  Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
