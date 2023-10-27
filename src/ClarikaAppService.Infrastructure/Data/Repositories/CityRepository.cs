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
    public class CityRepository : GenericRepository<City, long>, ICityRepository
    {
        public CityRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        //public CityRepository(IUnitOfWork context) : base(context)
        //{
        //}

        public override async Task<City> CreateOrUpdateAsync(City city)
        {
            List<Type> entitiesToBeUpdated = new List<Type>();
            return await base.CreateOrUpdateAsync(city, entitiesToBeUpdated);
        }
    }
}
