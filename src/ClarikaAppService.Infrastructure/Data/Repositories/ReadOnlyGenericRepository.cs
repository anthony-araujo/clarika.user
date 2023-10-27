using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using ClarikaAppService.Domain.Repositories.Interfaces;
using ClarikaAppService.Domain.Entities;
using Dapper;
using NPoco;
using System.Linq;
//using ServiceStack.OrmLite;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public abstract class ReadOnlyGenericRepository<TEntity, TKey> : IReadOnlyGenericRepository<TEntity, TKey>, IDisposable where TEntity : BaseEntity<TKey>
    {
        protected readonly IDbConnection _dbConnection;

        public ReadOnlyGenericRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public virtual async Task<TEntity> GetOneAsync(TKey id)
        {
            // Reemplaza "TableName" con el nombre real de la tabla en la base de datos
            var tableName = GetTableName<TEntity>();
            var query = $"SELECT * FROM {tableName} WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id });
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var tableName = GetTableName<TEntity>();
            var query = $"SELECT * FROM {tableName}";
            return await _dbConnection.QueryAsync<TEntity>(query);
        }

        public virtual async Task<IPage<TEntity>> GetPageAsync(IPageable pageable)
        {
            var tableName = GetTableName<TEntity>();
            var pageSql = $@"
            SELECT *
            FROM {tableName}
            ORDER BY Id 
            OFFSET {pageable.Offset} ROWS -- Salta las filas necesarias
            FETCH NEXT {pageable.PageSize} ROWS ONLY; -- Obtiene la cantidad de filas por página";

            var totalSql = $@"
            SELECT COUNT(*)
            FROM {tableName};";

            using (var multi = await _dbConnection.QueryMultipleAsync($"{pageSql}; {totalSql}"))
            {
                var results = await multi.ReadAsync<TEntity>();
                var total = await multi.ReadSingleAsync<int>();

                return new JHipsterNet.Core.Pagination.Page<TEntity>(results.ToList(), pageable, total);
            }
        }

        public virtual async Task<bool> Exists(TEntity entity)
        {
            var tableName = GetTableName<TEntity>();
            var sql = $"SELECT 1 FROM {tableName} WHERE Id = @Id";
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, new { entity.Id });
            return result == 1;
        }

        public virtual async Task<int> CountAsync()
        {
            var tableName = GetTableName<TEntity>();
            var query = $"SELECT COUNT(*) FROM {tableName}";
            return await _dbConnection.ExecuteScalarAsync<int>(query);
        }

        public virtual IFluentRepository<TEntity> QueryHelper()
        {
            return null;
            //return new FluentRepository<TEntity>(_dbConnection);
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }

        private string GetTableName<T>()
        {
            // Implementa la lógica para obtener el nombre de la tabla de manera segura
            // Esto puede variar dependiendo de cómo estés configurando tus entidades con Dapper.
            // Asegúrate de que este método retorne el nombre de la tabla correspondiente a la entidad.
            return typeof(T).Name;
        }

        private string BuildWhereClause(Expression<Func<TEntity, bool>> predicate)
        {
            // Implementa la lógica para convertir la expresión LINQ en una cláusula WHERE de SQL.
            // Esto puede ser un proceso complejo y depende de tus necesidades específicas.
            // Debes considerar cómo mapear las propiedades de la entidad a columnas de la tabla.
            // Asegúrate de que este método genere una cláusula WHERE válida en SQL.
            // Aquí proporciono un ejemplo simple, pero puede requerir una implementación más sofisticada.
            // Consulta la documentación de Dapper para obtener más detalles sobre cómo realizar consultas seguras.
            //var sql = SqlBuilder<TEntity>.BuildPredicate(predicate).ToSql();
            //return sql.sql;
            return $"{predicate.Body}";
        }

        
    }
}
