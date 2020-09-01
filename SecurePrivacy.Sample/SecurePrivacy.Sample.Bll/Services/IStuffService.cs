using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecurePrivacy.Sample.Model;

namespace SecurePrivacy.Sample.Bll.Services
{
    public interface IStuffService
    {
        Task<Stuff> CreateAsync(Stuff stuff);
        Task<Stuff> GetAsync(string id);
        Task<bool> UpdateAsync(string id, Stuff stuff);
        Task DeleteAsync(string id);
    }
}
