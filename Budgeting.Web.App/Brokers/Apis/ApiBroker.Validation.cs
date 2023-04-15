using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.ValidationProblemDetails;
using Newtonsoft.Json;
using System.Net;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker
    {
        public async static ValueTask ValidateHttpResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            bool isProblemDetailContent = IsProblemDetail(content);

            switch (isProblemDetailContent)
            {
                case true when httpResponseMessage.StatusCode == HttpStatusCode.BadRequest:
                    ValidationProblemDetail badRequestDetail = MapToProblemDetail(content);
                    throw new HttpResponseBadRequestException(httpResponseMessage, badRequestDetail);

                case false when httpResponseMessage.StatusCode == HttpStatusCode.BadRequest:
                    throw new HttpResponseBadRequestException(httpResponseMessage, content);

                case true when httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized:
                    ValidationProblemDetail UnauthorizedDetail = MapToProblemDetail(content);
                    throw new HttpResponseUnauthorizedException(httpResponseMessage, UnauthorizedDetail);

                case false when httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized:
                    throw new HttpResponseUnauthorizedException(httpResponseMessage, content);

                case true when httpResponseMessage.StatusCode == HttpStatusCode.NotFound:
                    ValidationProblemDetail NotFoundDetail = MapToProblemDetail(content);
                    throw new HttpResponseNotFoundException(httpResponseMessage, NotFoundDetail);

                case false when httpResponseMessage.StatusCode == HttpStatusCode.NotFound:
                    throw new HttpResponseNotFoundException(httpResponseMessage, content);

                case true when httpResponseMessage.StatusCode == HttpStatusCode.Conflict:
                    ValidationProblemDetail ConflictDetail = MapToProblemDetail(content);
                    throw new HttpResponseConflictException(httpResponseMessage, ConflictDetail);

                case false when httpResponseMessage.StatusCode == HttpStatusCode.Conflict:
                    throw new HttpResponseConflictException(httpResponseMessage, content);

                case true when httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError:
                    ValidationProblemDetail InternalServerErrorDetail = MapToProblemDetail(content);
                    throw new HttpResponseInternalServerErrorException(httpResponseMessage, InternalServerErrorDetail);

                case false when httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError:
                    throw new HttpResponseInternalServerErrorException(httpResponseMessage, content);
            }
        }

        private static ValidationProblemDetail MapToProblemDetail(string content) =>
            JsonConvert.DeserializeObject<ValidationProblemDetail>(content);

        private static bool IsProblemDetail(string content) =>
            content.ToLower().Contains("\"title\":") && content.ToLower().Contains("\"type\":");
    }
}
