using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;

namespace ClarikaAppService.Domain.Services;

public class LocationTypeService : ILocationTypeService
{
    protected readonly ILocationTypeRepository _locationTypeRepository;

    public LocationTypeService(ILocationTypeRepository locationTypeRepository)
    {
        _locationTypeRepository = locationTypeRepository;
    }

    public virtual async Task<LocationType> Save(LocationType locationType)
    {
        await _locationTypeRepository.CreateOrUpdateAsync(locationType);
        await _locationTypeRepository.SaveChangesAsync();
        return locationType;
    }

    public virtual async Task<IPage<LocationType>> FindAll(IPageable pageable)
    {
        var page = await _locationTypeRepository.QueryHelper()
            .GetPageAsync(pageable);
        return page;
    }

    public virtual async Task<LocationType> FindOne(long id)
    {
        var result = await _locationTypeRepository.QueryHelper()
            .GetOneAsync(locationType => locationType.Id == id);
        return result;
    }

    public virtual async Task Delete(long id)
    {
        await _locationTypeRepository.DeleteByIdAsync(id);
        await _locationTypeRepository.SaveChangesAsync();
    }
}
