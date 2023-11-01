using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ClarikaAppService.Infrastructure.Data.Repositories;

namespace ClarikaAppService.Domain.Services;

public class UserLocationService : IUserLocationService
{
    protected readonly IUserLocationRepository _userLocationRepository;

    public UserLocationService(IUserLocationRepository userLocationRepository)
    {
        _userLocationRepository = userLocationRepository;
    }

    public virtual async Task<UserLocation> Save(UserLocation userLocation)
    {
        await _userLocationRepository.CreateOrUpdateAsync(userLocation);
        //await _userLocationRepository.SaveChangesAsync();
        return userLocation;
    }

    public virtual async Task<IPage<UserLocation>> FindAll(IPageable pageable)
    {
        return await _userLocationRepository.GetPageAsync(pageable);
        //var page = await _userLocationRepository.QueryHelper()
        //    .Include(userLocation => userLocation.Country)
        //    .Include(userLocation => userLocation.LocationType)
        //    .Include(userLocation => userLocation.UserApp)
        //    .GetPageAsync(pageable);
        //return page;
    }

    public virtual async Task<UserLocation> FindOne(long id)
    {
        var result = await _userLocationRepository.GetOneAsync(id);
        return result;
    }

    public virtual async Task Delete(long id)
    {
        await _userLocationRepository.DeleteByIdAsync(id);
        //await _userLocationRepository.SaveChangesAsync();
    }
}
