using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Services.Interfaces
{
    public interface IStateService
    {
        Task<State> Save(State state);

        Task<IPage<State>> FindAll(IPageable pageable);

        Task<State> FindOne(long id);

        Task Delete(long id);
    }
}
