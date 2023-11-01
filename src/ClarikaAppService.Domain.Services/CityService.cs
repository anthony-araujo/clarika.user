using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Infrastructure.Data.Repositories;

namespace ClarikaAppService.Domain.Services;

public class CityService : ICityService
{
    protected readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public virtual async Task<City> Save(City city)
    {
        await _cityRepository.CreateOrUpdateAsync(city);
        await _cityRepository.SaveChangesAsync();
        return city;
    }

    public virtual async Task<IPage<City>> FindAll(IPageable pageable)
    {
        return await _cityRepository.GetPageAsync(pageable);
        //var page = await _cityRepository.QueryHelper()
        //    .Include(city => city.State)
        //    .GetPageAsync(pageable);
        //return page;
    }

    public virtual async Task<City> FindOne(long id)
    {
        var result = await _cityRepository.QueryHelper()
            .Include(city => city.State)
            .GetOneAsync(city => city.Id == id);
        return result;
    }

    public virtual async Task Delete(long id)
    {
        await _cityRepository.DeleteByIdAsync(id);
        await _cityRepository.SaveChangesAsync();
    }
}
