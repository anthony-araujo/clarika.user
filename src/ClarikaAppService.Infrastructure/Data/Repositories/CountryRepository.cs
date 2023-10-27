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
using Dapper;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public class CountryRepository : GenericRepository<Country, long>, ICountryRepository
    {
        public CountryRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
        public override async Task<Country> CreateOrUpdateAsync(Country country)
        {
            bool exists = await Exists(country);
            if (!exists) 
            {
                var sql = "INSERT INTO Country (Name) VALUES (@Name); SELECT SCOPE_IDENTITY()";
                var newId = await _dbConnection.ExecuteScalarAsync<long>(sql, new {Name = country.Name });
                country.Id = newId;
            }
            else
            {
                var sql = "UPDATE Country SET Name = @Name WHERE Id = @Id";
                await _dbConnection.ExecuteAsync(sql, new { Name = country.Name, Id = country.Id });
            }

            return country;
        }
    }
}
