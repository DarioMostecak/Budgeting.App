using Newtonsoft.Json;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly IConfiguration configuration;
        private HttpClient httpClient;

        public ApiBroker(IConfiguration configuration,
            HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = GetHttpClient(httpClient);
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl)
            where T : class
        {
            using var response = await this.httpClient.GetAsync(relativeUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async ValueTask<T> PostAsync<T>(string relativeUrl, T entity)
        {
            using var response = await this.httpClient.PostAsJsonAsync(relativeUrl, entity);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();

        }

        private async ValueTask<T> PutAsync<T>(string relativeUrl, T entity)
            where T : class
        {
            using var response = await this.httpClient.PutAsJsonAsync(relativeUrl, entity);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private async ValueTask<T> DeleteAsync<T>(string relativeUrl)
            where T : class
        {
            using var response = await this.httpClient.DeleteAsync(relativeUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private HttpClient GetHttpClient(HttpClient client)
        {
            var baseAddress = this.configuration["ApiBaseUrl"];
            client.BaseAddress = new Uri("https://localhost:7149/api/v1/");

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }


    }
}
