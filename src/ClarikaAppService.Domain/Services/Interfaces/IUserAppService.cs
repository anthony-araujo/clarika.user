using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using System.Collections.Generic;

namespace ClarikaAppService.Domain.Services.Interfaces
{
    public interface IUserAppService
    {
        Task<UserApp> Save(UserApp userApp);
        Task<IEnumerable<UserApp>> SaveAll(IEnumerable<UserApp> userApps);

        Task<IPage<UserApp>> FindAll(IPageable pageable);

        Task<UserApp> FindOne(long id);

        Task Delete(long id);
    }
}
