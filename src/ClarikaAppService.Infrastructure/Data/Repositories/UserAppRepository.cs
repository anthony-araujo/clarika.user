using System;
using System.Data;
using System.Threading.Tasks;
using ClarikaAppService.Crosscutting.Exceptions;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Repositories.Interfaces;
using Dapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public class UserAppRepository : GenericRepository<UserApp, long>, IUserAppRepository
    {
        public UserAppRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public override async Task<bool> Exists(UserApp userApp)
        {
            var tableName = typeof(UserApp).Name;
            var sql = $"SELECT 1 FROM {tableName} WHERE Id = @Id";
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, new { userApp.Id });
            return result == 1;
        }

        public override async Task<UserApp> CreateOrUpdateAsync(UserApp userApp)
        {
            var existingUser = await FindByEmailAsync(userApp.Email);

            if (existingUser != null)
            {
                if (existingUser.Id != userApp.Id)
                {
                    throw new BadRequestAlertException("Email address is already in use by another user.", "User App", "emailInUse");
                }
                // Here, the email is the same, and the user ID matches, which means it's an update.
                return Update(userApp);
            }
            else
            {
                bool exists = await Exists(userApp);

                if (userApp.Id.Equals(0) && exists)
                {
                    return Update(userApp);
                }
                else
                {
                    return Add(userApp);
                }
            }
        }

        public override UserApp Add(UserApp userApp)
        {
            var sql = "INSERT INTO userapp (FirstName, LastName, Email, DateBirth, Age, PasswordHash, SecurityStamp, ConcurrencyStamp, CountryId) " +
                      "VALUES (@FirstName, @LastName, @Email, @DateBirth, @Age, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @CountryId); " +
                      "SELECT SCOPE_IDENTITY()";

            var newId = _dbConnection.ExecuteScalar<long>(sql, userApp);
            userApp.Id = newId;
            return userApp;
        }

        public override UserApp Update(UserApp userApp)
        {
            var sql = "UPDATE userapp SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateBirth = @DateBirth, Age = @Age, " +
                      "PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, ConcurrencyStamp = @ConcurrencyStamp, CountryId = @CountryId " +
                      "WHERE Id = @Id";

            _dbConnection.Execute(sql, userApp);
            return userApp;
        }

        public async Task<UserApp> FindByEmailAsync(string email)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<UserApp>("SELECT * FROM userapp WHERE Email = @Email", new { Email = email });
        }

        
    }
}
