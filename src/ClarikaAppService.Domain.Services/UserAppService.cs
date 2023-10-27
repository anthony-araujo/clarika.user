using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;

namespace ClarikaAppService.Domain.Services;

public class UserAppService : IUserAppService
{
    protected readonly IUserAppRepository _userAppRepository;

    public UserAppService(IUserAppRepository userAppRepository)
    {
        _userAppRepository = userAppRepository;
    }

    public virtual async Task<UserApp> Save(UserApp userApp)
    {
        await _userAppRepository.CreateOrUpdateAsync(userApp);
        await _userAppRepository.SaveChangesAsync();
        return userApp;
    }

    public virtual async Task<IPage<UserApp>> FindAll(IPageable pageable)
    {
        var page = await _userAppRepository.QueryHelper()
            .Include(userApp => userApp.Country)
            .GetPageAsync(pageable);
        return page;
    }

    public virtual async Task<UserApp> FindOne(long id)
    {
        var result = await _userAppRepository.QueryHelper()
            .Include(userApp => userApp.Country)
            .GetOneAsync(userApp => userApp.Id == id);
        return result;
    }

    public virtual async Task Delete(long id)
    {
        await _userAppRepository.DeleteByIdAsync(id);
        await _userAppRepository.SaveChangesAsync();
    }
}
