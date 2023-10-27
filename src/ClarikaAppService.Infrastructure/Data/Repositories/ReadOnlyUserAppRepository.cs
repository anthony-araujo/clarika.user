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
    public class ReadOnlyUserAppRepository : ReadOnlyGenericRepository<UserApp, long>, IReadOnlyUserAppRepository
    {
        //public ReadOnlyUserAppRepository(IUnitOfWork context) : base(context)
        //{
        //}
        public ReadOnlyUserAppRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
