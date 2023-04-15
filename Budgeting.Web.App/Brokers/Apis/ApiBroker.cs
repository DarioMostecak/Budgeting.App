namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly HttpClient httpClient;

        public ApiBroker(HttpClient httpClient)
        {
            this.httpClient = GetHttpClient(httpClient);
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

            await ValidateHttpResponseAsync(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        public async ValueTask<TResult> PostContentAsync<TContent, TResult>(
            string relativeUrl,
            TContent content,
            string mediaType = "text/json")
        {
            HttpContent contentString = ConvertToHttpContent(content, mediaType);

            HttpResponseMessage responseMessage =
               await this.httpClient.PostAsync(relativeUrl, contentString);

            await ValidateHttpResponseAsync(responseMessage);

            return await DeserializeResponseContent<TResult>(responseMessage);
        }

        private async ValueTask<T> PutContentAsync<T>(string relativeUrl,
            T content,
            string mediaType = "text/json")
        {
            HttpContent contentString = ConvertToHttpContent(content, mediaType);

            HttpResponseMessage responseMessage =
               await this.httpClient.PutAsync(relativeUrl, contentString);

            await ValidateHttpResponseAsync(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        private async ValueTask<T> DeleteContentAsync<T>(string relativeUrl)
        {
            HttpResponseMessage responseMessage = await
                this.httpClient.DeleteAsync(relativeUrl);

            await ValidateHttpResponseAsync(responseMessage);

            return await DeserializeResponseContent<T>(responseMessage);
        }

        private HttpClient GetHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7149/api/v1");
            //add header data

            return httpClient;
        }
    }
}
