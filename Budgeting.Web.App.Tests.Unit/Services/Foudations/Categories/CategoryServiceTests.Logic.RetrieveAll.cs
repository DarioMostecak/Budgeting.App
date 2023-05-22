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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllCategoryAsync()
        {
            //given
            DateTime randomDate = GetRandomDateTime();
            IEnumerable<Category> categories = CreateRandomCategories(randomDate);
            IEnumerable<Category> retrieveCategories = categories;
            IEnumerable<Category> expectedCategories = categories.DeepClone();

            this.apiBrokerMock.Setup(broker =>
                broker.GetCategoriesAsync())
                        .ReturnsAsync(retrieveCategories);

            //when
            IEnumerable<Category> actualCategories =
                await this.categoryService.RetrieveAllCategoriesAsync();

            //then
            actualCategories.Should().BeEquivalentTo(expectedCategories);

            this.apiBrokerMock.Verify(broker =>
               broker.GetCategoriesAsync(),
                  Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
