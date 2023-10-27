using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using ClarikaAppService.Domain.Entities;

namespace ClarikaAppService.Domain.Repositories.Interfaces;

public interface IReadOnlyGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity> GetOneAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IPage<TEntity>> GetPageAsync(IPageable pageable);
    Task<bool> Exists(TEntity entity);
    Task<int> CountAsync();
    IFluentRepository<TEntity> QueryHelper();
}
