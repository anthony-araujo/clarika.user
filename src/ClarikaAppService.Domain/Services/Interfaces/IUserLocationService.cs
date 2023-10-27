using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Services.Interfaces
{
    public interface IUserLocationService
    {
        Task<UserLocation> Save(UserLocation userLocation);

        Task<IPage<UserLocation>> FindAll(IPageable pageable);

        Task<UserLocation> FindOne(long id);

        Task Delete(long id);
    }
}
