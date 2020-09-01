using System.Threading.Tasks;
using SecurePrivacy.Sample.Bll.Services;
using SecurePrivacy.Sample.Dal.Repositories;
using SecurePrivacy.Sample.Model;

namespace SecurePrivacy.Sample.Bll.Impl.services
{
    public class StuffService : IStuffService
    {
        private readonly IStuffRepository _stuffRepository;

        public StuffService(IStuffRepository stuffRepository)
        {
            _stuffRepository = stuffRepository;
        }

        public async Task<Stuff> CreateAsync(Stuff stuff)
        {
            return await _stuffRepository.Insert(stuff);
        }

        public async Task DeleteAsync(string id)
        {
            await _stuffRepository.Delete(id);
        }

        public async Task<Stuff> GetAsync(string id)
        {
            return await _stuffRepository.GetById(id);
        }

        public async Task<bool> UpdateAsync(string id, Stuff stuff)
        {
            return await _stuffRepository.Update(id, stuff);
        }
    }
}
