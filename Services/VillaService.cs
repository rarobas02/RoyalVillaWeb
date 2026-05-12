using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : IVillaService
    {
        public Task<T?> CreateAsync<T>(VillaCreateDTO dto, string token)
        {
            throw new NotImplementedException();
        }

        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAllAsync<T>(string token)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            throw new NotImplementedException();
        }
    }
}
