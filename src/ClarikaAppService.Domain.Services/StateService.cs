using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Services.Interfaces;
using ClarikaAppService.Domain.Repositories.Interfaces;

namespace ClarikaAppService.Domain.Services;

public class StateService : IStateService
{
    protected readonly IStateRepository _stateRepository;

    public StateService(IStateRepository stateRepository)
    {
        _stateRepository = stateRepository;
    }

    public virtual async Task<State> Save(State state)
    {
        await _stateRepository.CreateOrUpdateAsync(state);
        await _stateRepository.SaveChangesAsync();
        return state;
    }

    public virtual async Task<IPage<State>> FindAll(IPageable pageable)
    {
        var page = await _stateRepository.QueryHelper()
            .Include(state => state.Country)
            .GetPageAsync(pageable);
        return page;
    }

    public virtual async Task<State> FindOne(long id)
    {
        var result = await _stateRepository.QueryHelper()
            .Include(state => state.Country)
            .GetOneAsync(state => state.Id == id);
        return result;
    }

    public virtual async Task Delete(long id)
    {
        await _stateRepository.DeleteByIdAsync(id);
        await _stateRepository.SaveChangesAsync();
    }
}
