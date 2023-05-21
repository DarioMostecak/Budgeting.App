// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------


using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Services.Foundations.Categories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Tynamix.ObjectFiller;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        private readonly Mock<IApiBroker> apiBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICategoryService categoryServiceMock;

        public CategoryServiceTests()
        {
            this.apiBrokerMock = new Mock<IApiBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.categoryServiceMock = new CategoryService(
                apiBroker: this.apiBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        private static Expression<Func<ExceptionModel, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);
        }

        public static TheoryData DependencyApiException()
        {
            string exceptionMessage = GetRandomString();
            var responseMessage = new HttpResponseMessage();

            var httpResponseInternalserverError =
                new HttpResponseInternalServerErrorException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var httpRequestException =
                new HttpRequestException();

            var httpResponseException =
                new HttpResponseException(
                    httpResponseMessage: responseMessage,
                    message: exceptionMessage);

            return new TheoryData<Exception>
            {
                httpResponseInternalserverError,
                httpResponseException,
                httpRequestException
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static DateTime GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Category CreateRandomCategory() =>
            CreateRandomCategoryFiller(dates: DateTime.UtcNow).Create();

        private static IEnumerable<Category> CreateRandomCategories(DateTime dates) =>
           CreateRandomCategoryFiller(dates).Create(GetRandomNumber()).AsEnumerable();

        private static Filler<Category> CreateRandomCategoryFiller(DateTime dates)
        {
            var filler = new Filler<Category>();
            Guid categoryId = Guid.NewGuid();

            filler.Setup()
                .OnProperty(category => category.CategoryId).Use(categoryId)
                .OnProperty(category => category.Title).Use("Lunch")
                .OnProperty(category => category.Type).Use("Expense")
                .OnProperty(category => category.TimeCreated).Use(dates)
                .OnProperty(category => category.TimeModify).Use(dates.AddDays(10));

            return filler;
        }
    }
}
