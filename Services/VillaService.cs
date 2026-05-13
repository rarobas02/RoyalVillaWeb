using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly string _villUrl;
        private const string APIEndpoint = "/api/villa";
        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration, string villUrl) : base(httpClient)
        {
            _villUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T?> CreateAsync<T>(VillaCreateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{_villUrl}{APIEndpoint}",
                Token = token
            });
        }
        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{_villUrl}{APIEndpoint}/{id}",
                Token = token
            });
        }
        public Task<T?> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{_villUrl}{APIEndpoint}",
                Token = token
            });
        }
        public Task<T?> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = $"{_villUrl}{APIEndpoint}/{id}",
                Token = token
            });
        }
        public Task<T?> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{_villUrl}{APIEndpoint}/{dto.id}",
                Token = token
            });
        }
    }
}
