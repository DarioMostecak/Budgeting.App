using Newtonsoft.Json;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker : IApiBroker
    {
        private readonly IConfiguration configuration;
        private HttpClient httpClient;
        private IHttpClientFactory httpClientFactory { get; set; }

        public ApiBroker(IConfiguration configuration,
            HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            this.httpClient = GetHttpClient(httpClient);

        }


        private async ValueTask<T> GetAsync<T>(string relativeUrl)
            where T : class
        {
            var client = this.httpClientFactory.CreateClient("BudgetApi");
            using var response = await client.GetAsync(relativeUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private async ValueTask<T> PostAsync<T>(string relativeUrl, T entity)
        {
            var client = this.httpClientFactory.CreateClient("BudgetApi");
            using var response = await client.PostAsJsonAsync(relativeUrl, entity);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);

        }

        private async ValueTask<T> PutAsync<T>(string relativeUrl, T entity)
            where T : class
        {
            var client = this.httpClientFactory.CreateClient("BudgetApi");
            using var response = await client.PutAsJsonAsync(relativeUrl, entity);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private async ValueTask<T> DeleteAsync<T>(string relativeUrl)
            where T : class
        {
            var client = this.httpClientFactory.CreateClient("BudgetApi");
            using var response = await client.DeleteAsync(relativeUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private HttpClient GetHttpClient(HttpClient client)
        {
            var baseAddress = this.configuration["ApiBaseUrl"];
            client.BaseAddress = new Uri(baseAddress);

            client.DefaultRequestHeaders.Add("Accept", "application/json");


            return client;
        }


    }
}
