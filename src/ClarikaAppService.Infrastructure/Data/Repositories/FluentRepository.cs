using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClarikaAppService.Domain.Repositories.Interfaces;
using Dapper;
using JHipsterNet.Core.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public class FluentRepository<TEntity> : IFluentRepository<TEntity> where TEntity : class
    {
        private readonly IDbConnection _connection;
        private List<Expression<Func<TEntity, object>> _includeProperties;

        public FluentRepository(IDbConnection connection)
        {
            _connection = connection;
            _includeProperties = new List<Expression<Func<TEntity, object>>();
        }

        public IFluentRepository<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            // Construct your SQL query here based on the filter
            // Example:
            // var query = "SELECT * FROM YourTable WHERE " + filter.ToSql();
            return await _connection.QuerySingleOrDefaultAsync<TEntity>(query, filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            // Construct your SQL query to fetch all records
            // Example:
            // var query = "SELECT * FROM YourTable";
            return await _connection.QueryAsync<TEntity>(query);
        }

        public IFluentRepository<TEntity> Include(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include) => throw new NotImplementedException();
        public IFluentRepository<TEntity> AsNoTracking() => throw new NotImplementedException();
        public INoSqlFluentRepository<TEntity> Filter(Expression<Func<TEntity, bool>> filter) => throw new NotImplementedException();
        public INoSqlFluentRepository<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy) => throw new NotImplementedException();
        public Task<IPage<TEntity>> GetPageAsync(IPageable pageable) => throw new NotImplementedException();

        // Implement other repository methods as needed

        // ...

        // No need for BuildQuery method or EF-specific features like tracking, ordering, and including.

        // ...
    }
}
