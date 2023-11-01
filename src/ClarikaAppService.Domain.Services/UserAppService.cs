using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System;

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
        return userApp;
    }
    public async Task<IEnumerable<UserApp>> SaveAll(IEnumerable<UserApp> userApps)
    {
        // Start a database transaction
        //using (var transaction = _userAppRepository.BeginTransaction())
        //{
            try
            {
                foreach (var userApp in userApps)
                {
                    await _userAppRepository.CreateOrUpdateAsync(userApp);
                }

                // Commit the transaction if all operations are successful
                //transaction.Commit();
            }
            catch (Exception)
            {
                // Rollback the transaction in case of an error
                //transaction.Rollback();
                throw; // Re-throw the exception for handling at a higher level
            }

            return userApps;
        //}
    }


    public virtual async Task<IPage<UserApp>> FindAll(IPageable pageable)
    {
        return await _userAppRepository.GetPageAsync(pageable);
    }

    public virtual async Task<UserApp> FindOne(long id)
    {
        return await _userAppRepository.GetOneAsync(id);
    }

    public virtual async Task Delete(long id)
    {
        await _userAppRepository.DeleteByIdAsync(id);
    }
}
