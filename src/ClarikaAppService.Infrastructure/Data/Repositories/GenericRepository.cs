using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using ClarikaAppService.Domain.Entities;
using ClarikaAppService.Domain.Repositories.Interfaces;
using System.Data;
using Dapper;

namespace ClarikaAppService.Infrastructure.Data.Repositories
{
    public abstract class GenericRepository<TEntity, TKey> : ReadOnlyGenericRepository<TEntity, TKey>, IGenericRepository<TEntity, TKey>, IDisposable where TEntity : BaseEntity<TKey>
    {
        protected IDbConnection _dbConnection;

        protected GenericRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public virtual TEntity Add(TEntity entity)
        {
            // Dapper: You don't need to add entities in Dapper explicitly.
            return entity;
        }

        public virtual bool AddRange(params TEntity[] entities)
        {
            // Dapper: You don't need to add entities in Dapper explicitly.
            return true;
        }

        public virtual TEntity Attach(TEntity entity)
        {
            // Dapper: Dapper doesn't use explicit entity state management.
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            // Dapper: Use Dapper's Update method to update the entity.
            var updateSql = $"UPDATE {typeof(TEntity).Name} SET /* specify the update fields and parameters */ WHERE Id = @Id";
            _dbConnection.Execute(updateSql, entity);
            return entity;
        }

        public virtual TEntity Update(string id, TEntity entity)
        {
            // Dapper: Use Dapper's Update method to update the entity.
            var updateSql = $"UPDATE {typeof(TEntity).Name} SET /* specify the update fields and parameters */ WHERE Id = @Id";
            _dbConnection.Execute(updateSql, entity);
            return entity;
        }

        public virtual bool UpdateRange(params TEntity[] entities)
        {
            // Dapper: Use Dapper's Update method for each entity in the range.
            foreach (var entity in entities)
            {
                Update(entity);
            }
            return true;
        }

        public async virtual Task<TEntity> CreateOrUpdateAsync(TEntity entity)
        {
            bool exists = await Exists(entity);
            if (entity.Id.Equals(0) && exists)
            {
                Update(entity);
            }
            else
            {
                // Perform an insert operation using Dapper
                var insertSql = $"INSERT INTO {typeof(TEntity).Name} (/* specify columns */) VALUES (/* specify values */); SELECT SCOPE_IDENTITY()";
                entity.Id = await _dbConnection.ExecuteScalarAsync<TKey>(insertSql, entity);
            }
            return entity;
        }

        public async virtual Task<TEntity> CreateOrUpdateAsync(TEntity entity, ICollection<Type> entitiesToBeUpdated)
        {
            // Dapper: Handle updates for entities explicitly
            bool exists = await Exists(entity);
            if (entity.Id.Equals(0) && exists)
            {
                Update(entity);
            }
            else
            {
                // Perform an insert operation using Dapper
                var insertSql = $"INSERT INTO {typeof(TEntity).Name} (/* specify columns */) VALUES (/* specify values */); SELECT SCOPE_IDENTITY()";
                entity.Id = await _dbConnection.ExecuteScalarAsync<TKey>(insertSql, entity);
            }
            return entity;
        }

        public virtual async Task Clear()
        {
            // Dapper: Dapper doesn't require removing all entities explicitly.
        }

        public virtual async Task DeleteByIdAsync(TKey id)
        {
            // Dapper: Use Dapper to delete the entity by ID.
            var deleteSql = $"DELETE FROM {typeof(TEntity).Name} WHERE Id = @Id";
            _dbConnection.Execute(deleteSql, new { Id = id });
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            // Dapper: Use Dapper to delete the entity by ID.
            DeleteByIdAsync(entity.Id);
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dapper: Dapper doesn't require explicit SaveChangesAsync.
            return 0;
        }

        protected async Task RemoveManyToManyRelationship(string joinEntityName, string ownerIdKey, string ownedIdKey, long ownerEntityId, List<long> idsToIgnore)
        {
            // Dapper: Remove many-to-many relationships using DELETE statements.
            var deleteSql = $"DELETE FROM {joinEntityName} WHERE {ownerIdKey} = @OwnerEntityId AND {ownedIdKey} NOT IN @IdsToIgnore";
            _dbConnection.Execute(deleteSql, new { OwnerEntityId = ownerEntityId, IdsToIgnore = idsToIgnore });
        }
    }
}
