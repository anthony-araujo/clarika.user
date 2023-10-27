using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Services.Interfaces
{
    public interface ILocationTypeService
    {
        Task<LocationType> Save(LocationType locationType);

        Task<IPage<LocationType>> FindAll(IPageable pageable);

        Task<LocationType> FindOne(long id);

        Task Delete(long id);
    }
}
