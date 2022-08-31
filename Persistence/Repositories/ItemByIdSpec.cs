using System.Linq.Expressions;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManager.Persistence.Repositories;

public class EntityFilterSpec<T> : Specification<T> where T : class
{
    public EntityFilterSpec(string key, string value,
        params Expression<Func<T, object>>[] includes)
    {
        Query.Where(w => EF.Property<string>(w, key).Contains(value));
        includes.Aggregate(Query, (current, include) => current.Include(include))
            .Where(w => EF.Property<string>(w, key).Contains(value));
    }
}