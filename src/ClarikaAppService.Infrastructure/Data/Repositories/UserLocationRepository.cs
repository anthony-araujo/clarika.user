using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Infrastructure.Data.Extensions;
using System.Data;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public class UserLocationRepository : GenericRepository<UserLocation, long>, IUserLocationRepository
    {
        public UserLocationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        //public UserLocationRepository(IUnitOfWork context) : base(context)
        //{
        //}

        public override async Task<UserLocation> CreateOrUpdateAsync(UserLocation userLocation)
        {
            List<Type> entitiesToBeUpdated = new List<Type>();
            return await base.CreateOrUpdateAsync(userLocation, entitiesToBeUpdated);
        }
    }
}
