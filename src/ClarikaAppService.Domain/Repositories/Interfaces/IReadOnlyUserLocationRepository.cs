
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Repositories.Interfaces
{

    public interface IReadOnlyUserLocationRepository : IReadOnlyGenericRepository<UserLocation, long>
    {
    }

}
