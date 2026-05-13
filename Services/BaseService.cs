using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Text.Json;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _httpClient { get; set; }
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public ApiResponse<object> ResponseModel { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.ResponseModel = new();
            this._httpClient = httpClient;
        }
        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("RoyalVillaAPI"); // "RoyalVillaAPI" is the name of the HttpClient configured in Program.cs
                var message = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiRequest.Url, UriKind.Relative), // Set the request URI from the ApiRequest
                    Method = GetHttpMethod(apiRequest.ApiType)
                }; // Create a new HttpRequestMessage

                if(apiRequest.Data != null)
                {
                    message.Content = JsonContent.Create(apiRequest.Data, options: JsonOptions);
                } // If there is data to send, serialize it to JSON and set it as the content of the request
                var apiResponse = await client.SendAsync(message); // Send the HTTP request and await the response
                return await apiResponse.Content.ReadFromJsonAsync<T>(JsonOptions); // Read the response content as JSON and deserialize it to the specified type T
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return default;
            }
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
