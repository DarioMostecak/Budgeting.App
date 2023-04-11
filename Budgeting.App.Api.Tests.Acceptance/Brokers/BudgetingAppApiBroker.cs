using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Extensions;
using Budgeting.App.Api.Tests.Acceptance.Models.IdentityRequests;
using Budgeting.App.Api.Tests.Acceptance.Models.IdentityResponses;
using Budgeting.App.Api.Tests.Acceptance.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;

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

        public async ValueTask<TResult> PostContentAsync<TContent, TResult>(
            string relativeUrl,
            TContent content,
            string mediaType = "text/json")
        {
            HttpContent contentString = ConvertToHttpContent(content, mediaType);

            HttpResponseMessage responseMessage =
               await this.httpClient.PostAsync(relativeUrl, contentString);

            return await DeserializeResponseContent<TResult>(responseMessage);
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


        #region Authentication Header
        public async Task<User> AddAuthenticationHeaderAsync(User user = null, string password = null)
        {
            if (user is null && password is null)
            {
                user = CreateRandomUser();
                password = GetRandomPassword();

                await this.PostUserAsync(user, password);
            }

            IdentityResponse identityResponse =
                CreateIdentityResponse(user);

            this.httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", identityResponse.Token);

            return user;
        }

        public async Task RemoveAuthenticationHeaderAsync(User user = null)
        {
            if (user != null)
                await this.DeleteUserAsync(user.Id);

            this.httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();

        private static User CreateRandomUser() =>
            new User
            {
                Id = Guid.NewGuid(),
                Email = "maylo@mail.com",
                FirstName = "Maylo",
                LastName = "Kongo",
                CreatedDate = DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)),
                UpdatedDate = DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1)).AddHours(1),
            };

        private static IdentityRequest CreateIdentityRequest(User user, string password) =>
            new IdentityRequest
            {
                Email = user.Email,
                Password = password,
            };

        #endregion

        #region Token Handler
        private JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        private SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = GetTokenDescriptor(identity);

            return TokenHandler.CreateToken(tokenDescriptor);
        }

        private string WriteToken(SecurityToken token)
        {
            return TokenHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claims)
        {
            byte[] key = Encoding.ASCII.GetBytes("f5422e6cdfde4af3bf631c7dd1f80b97");

            return new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                Audience = "SwaggerUI",
                Issuer = "BudgetingAppApi",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                   , SecurityAlgorithms.HmacSha256Signature)

            };
        }

        private IdentityResponse CreateIdentityResponse(User user)
        {
            var identityResponse = new IdentityResponse();
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserFullName", user.FirstName + " " + user.LastName)
            });

            var token = CreateSecurityToken(claimsIdentity);
            identityResponse.Token = WriteToken(token);

            return identityResponse;
        }
        #endregion

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
