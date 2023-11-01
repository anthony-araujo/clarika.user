using ClarikaAppService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ClarikaAppService.Domain.Repositories.Interfaces
{
    public interface IUserAppRepository : IGenericRepository<UserApp, long>
    {
        Task<UserApp> FindByEmailAsync(string email);
       
    }
}
