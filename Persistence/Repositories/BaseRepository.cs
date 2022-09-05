using System.Linq.Expressions;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WarehouseManager.Core.DTOs;
using WarehouseManager.Persistence.Context;
using WarehouseManager.Persistence.Interfaces;

namespace WarehouseManager.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _table;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _table = context.Set<T>();
    }

    public async Task<List<T>> EntityFilterSpec(EntityFilterSpec<T> spec)
    {
        return await _table.WithSpecification(spec).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null, string? includes = null)
    {
        if (predicate == null) return await _table.ToListAsync();

        var query = _table.Where(predicate);

        if (includes == null) return await query.ToListAsync();

        query = includes.Split(",").Aggregate(query, (current, str) => current.Include(str).AsQueryable());

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllIncluded(Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includes)
    {
        if (predicate == null) return await _table.ToListAsync();

        var query = _table.Where(predicate);

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.ToListAsync();
    }

    public async Task<PagedResultDto<T>> GetAllIncludedPagination(
        Expression<Func<T, bool>>? predicate = null,
        int pageIndex = 1,
        int pageSize = 10,
        params Expression<Func<T, object>>[] includes)
    {
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize <= 0) pageSize = 10;

        IQueryable<T> query = _table;

        if (predicate != null) query = _table.Where(predicate);

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return new PagedResultDto<T>
        {
            TotalCount = await query.CountAsync(),
            Result = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
        };
    }

    public async Task<T?> GetById(object id) => await _table.FindAsync(id);

    public void Add(T obj) => _table.Add(obj);

    public void Update(T obj)
    {
        _table.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(T obj)
    {
        // _table.Remove(obj);
        _context.Set<T>().Remove(obj);
        // _context.Entry(obj).State = EntityState.Deleted;
    }

    public Task DeleteRange(IEnumerable<T> entities)
    {
        _table.RemoveRange(entities);
        return Task.CompletedTask;
    }
}