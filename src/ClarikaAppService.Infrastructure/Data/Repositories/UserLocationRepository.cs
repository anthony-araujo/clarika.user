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
            bool exists = await Exists(userLocation);
            if (!exists)
            {
                    // Insert a new UserLocation
                    var insertQuery = @"
                INSERT INTO [dbo].[user_location]
                ([Address]
                 ,[ZipCode]
                 ,[Province]
                 ,[CountryId]
                 ,[LocationTypeId]
                 ,[UserAppId])
                VALUES
                (@Address, @ZipCode, @Province, @CountryId, @LocationTypeId, @UserAppId);
                SELECT SCOPE_IDENTITY()";

                    // Execute the insert query and retrieve the new Id
                    userLocation.Id = await _dbConnection.ExecuteScalarAsync<long>(insertQuery, userLocation);
            }
            else
            {
                    // Update an existing UserLocation
                    var updateQuery = @"
                UPDATE [dbo].[user_location]
                SET [Address] = @Address,
                    [ZipCode] = @ZipCode,
                    [Province] = @Province,
                    [CountryId] = @CountryId,
                    [LocationTypeId] = @LocationTypeId,
                    [UserAppId] = @UserAppId
                WHERE [Id] = @Id";

                    // Execute the update query
                    await _dbConnection.ExecuteAsync(updateQuery, userLocation);
            }
            return userLocation;
        }
    }
}
