using System.Linq;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Infrastructure.Data.Extensions;
using System.Data;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public class ReadOnlyUserLocationRepository : ReadOnlyGenericRepository<UserLocation, long>, IReadOnlyUserLocationRepository
    {
        //public ReadOnlyUserLocationRepository(IUnitOfWork context) : base(context)
        //{
        //}
        public ReadOnlyUserLocationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
