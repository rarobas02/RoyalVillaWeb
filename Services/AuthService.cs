using Humanizer;
using NuGet.Common;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private const string APIEndpoint = "/auth";
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
        }

        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = APIEndpoint + "/login"
            });
        }

        public Task<T?> RegisterAsync<T>(RegistrationRequestDTO registerRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = registerRequestDTO,
                Url = APIEndpoint + "/register"
            });
        }
    }
}
