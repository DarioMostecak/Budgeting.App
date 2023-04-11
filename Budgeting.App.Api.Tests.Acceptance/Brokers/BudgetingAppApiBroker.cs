using Budgeting.App.Api.Brokers.Storages;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Budgeting.App.Api.Tests.Acceptance.Brokers
{
    public partial class BudgetingAppApiBroker
    {
        public readonly WebApplicationFactory<StorageBroker> webApplicationFactory;
        public readonly HttpClient httpClient;

        public BudgetingAppApiBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<StorageBroker>();
            this.httpClient = this.webApplicationFactory.CreateClient();
        }

        private async ValueTask<T> GetContentAsync<T>(string relativeUrl)
        {
            HttpResponseMessage responseMessage =
                await this.httpClient.GetAsync(relativeUrl);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        private async ValueTask<T> PostContentAsync<T>(
            string relativeUrl,
            T content,
            string mediaType = "text/json")
        {
            HttpContent contentString = ConvertToHttpContent(content, mediaType);

            HttpResponseMessage responseMessage =
               await this.httpClient.PostAsync(relativeUrl, contentString);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        private async ValueTask<T> PutContentAsync<T>(string relativeUrl,
            T content,
            string mediaType = "text/json")
        {
            HttpContent contentString = ConvertToHttpContent(content, mediaType);

            HttpResponseMessage responseMessage =
               await this.httpClient.PutAsync(relativeUrl, contentString);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        private async ValueTask<T> DeleteContentAsync<T>(string relativeUrl)
        {
            HttpResponseMessage responseMessage = await
                this.httpClient.DeleteAsync(relativeUrl);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        #region Utilites
        private static HttpContent ConvertToHttpContent<T>(T content, string mediaType)
        {
            return mediaType switch
            {
                "text/json" => ConvertToJsonStringContent(content, mediaType),
                "text/plain" => ConvertToStringContent(content, mediaType),
                "application/octet-stream" => ConvertToStreamContent(content as Stream, mediaType),
                _ => ConvertToStringContent(content, mediaType)
            };
        }

        private static StringContent ConvertToStringContent<T>(T content, string mediaType)
        {
            return new StringContent(
                content: content.ToString(),
                encoding: Encoding.UTF8,
                mediaType);
        }

        private static StringContent ConvertToJsonStringContent<T>(T content, string mediaType)
        {
            string serializedRestrictionRequest = JsonConvert.SerializeObject(content);

            var contentString =
                new StringContent(
                    content: serializedRestrictionRequest,
                    encoding: Encoding.UTF8,
                    mediaType);

            return contentString;
        }

        private static StreamContent ConvertToStreamContent<T>(T content, string mediaType)
            where T : Stream
        {
            var contentStream = new StreamContent(content);
            contentStream.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return contentStream;
        }

        private static async ValueTask<T> DeserializeResponseContent<T>(HttpResponseMessage responseMessage)
        {
            string responseString = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }
        #endregion
    }
}
