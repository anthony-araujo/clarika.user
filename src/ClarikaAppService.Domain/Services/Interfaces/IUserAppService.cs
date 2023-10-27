using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Services.Interfaces
{
    public interface IUserAppService
    {
        Task<UserApp> Save(UserApp userApp);

        Task<IPage<UserApp>> FindAll(IPageable pageable);

        Task<UserApp> FindOne(long id);

        Task Delete(long id);
    }
}
