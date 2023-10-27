using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Entities.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace ClarikaAppService.Infrastructure.Data
{
    public class ApplicationDatabaseContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbConnection _dbConnection;

        public ApplicationDatabaseContext(IDbConnection dbConnection, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnection = dbConnection;
            _httpContextAccessor = httpContextAccessor;
        }

        // Ejemplo de método para obtener un usuario por su ID
        public async Task<UserApp> GetUserByIdAsync(int userId)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<UserApp>("SELECT * FROM UserApps WHERE Id = @UserId", new { UserId = userId });
        }

        // Ejemplo de método para agregar un nuevo usuario
        public async Task<int> AddUserAsync(UserApp user)
        {
            // Realizar la inserción en la base de datos
            // Asegúrate de que la entidad 'user' esté configurada correctamente
            // y que los parámetros se pasen de manera segura
            var query = "INSERT INTO UserApps (Name, Email) VALUES (@Name, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";
            var result = await _dbConnection.ExecuteScalarAsync<int>(query, user);
            return result;
        }

        // Ejemplo de método para actualizar un usuario
        public async Task<bool> UpdateUserAsync(UserApp user)
        {
            // Realizar la actualización en la base de datos
            // Asegúrate de que la entidad 'user' esté configurada correctamente
            // y que los parámetros se pasen de manera segura
            var query = "UPDATE UserApps SET Name = @Name, Email = @Email WHERE Id = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, user);
            return rowsAffected > 0;
        }

        // Ejemplo de método para eliminar un usuario
        public async Task<bool> DeleteUserAsync(int userId)
        {
            // Realizar la eliminación en la base de datos
            var query = "DELETE FROM UserApps WHERE Id = @UserId";
            var rowsAffected = await _dbConnection.ExecuteAsync(query, new { UserId = userId });
            return rowsAffected > 0;
        }

        // Agrega otros métodos para interactuar con otras entidades de la base de datos

        // No necesitas OnModelCreating en este caso.
    }
}
