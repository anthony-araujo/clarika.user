
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Repositories.Interfaces
{

    public interface IReadOnlyCountryRepository : IReadOnlyGenericRepository<Country, long>
    {
    }

}
