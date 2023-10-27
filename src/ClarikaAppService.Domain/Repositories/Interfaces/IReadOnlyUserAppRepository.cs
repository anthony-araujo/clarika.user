
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Repositories.Interfaces
{

    public interface IReadOnlyUserAppRepository : IReadOnlyGenericRepository<UserApp, long>
    {
    }

}
