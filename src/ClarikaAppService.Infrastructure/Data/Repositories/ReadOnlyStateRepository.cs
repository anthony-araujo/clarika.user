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
    public class ReadOnlyStateRepository : ReadOnlyGenericRepository<State, long>, IReadOnlyStateRepository
    {
        //public ReadOnlyStateRepository(IUnitOfWork context) : base(context)
        //{
        //}
        public ReadOnlyStateRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
