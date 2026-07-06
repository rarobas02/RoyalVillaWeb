using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _httpClient { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public ApiResponse<object> ResponseModel { get; set; }
        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.ResponseModel = new();
            this._httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            var client = _httpClient.CreateClient("RoyalVillaAPI");
            using var message = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.Url, UriKind.Relative),
                Method = GetHttpMethod(apiRequest.ApiType)
            };

            var token = _httpContextAccessor.HttpContext?.Session.GetString(SD.SessionToken);

            if(!string.IsNullOrEmpty(token))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (apiRequest.Data != null)
            {
                message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions);
            }

            var apiResponse = await client.SendAsync(message);
            if (!apiResponse.IsSuccessStatusCode)
            {
                var errorContent = await apiResponse.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {(int)apiResponse.StatusCode}: {apiResponse.ReasonPhrase}. Response: {errorContent}");
            }

            var result = await apiResponse.Content.ReadFromJsonAsync<T>(JsonOptions);
            if (result is null)
            {
                throw new InvalidOperationException("Response content was empty or could not be deserialized.");
            }

            return result;
        }
        private static HttpMethod GetHttpMethod(SD.ApiType apiType)
        {
            // Map the SD.ApiType to the corresponding HttpMethod
            return apiType switch
            {
                SD.ApiType.GET => HttpMethod.Get,
                SD.ApiType.POST => HttpMethod.Post,
                SD.ApiType.PUT => HttpMethod.Put,
                SD.ApiType.DELETE => HttpMethod.Delete,
                _ => throw new ArgumentOutOfRangeException(nameof(apiType), $"Unsupported API type: {apiType}")
            };
        }
    }
}
