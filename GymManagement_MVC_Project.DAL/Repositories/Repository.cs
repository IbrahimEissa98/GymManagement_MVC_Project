using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagement_MVC_Project.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly GymDbContext _gymDbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(GymDbContext gymDbContext)
    {
        _gymDbContext = gymDbContext;
        _dbSet = gymDbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllIncludingDeletedAsync(CancellationToken ct = default)
    {
        return await _dbSet.AsNoTracking().IgnoreQueryFilters().OrderBy(t => t.IsDeleted).ToListAsync(ct);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TEntity?> GetByIdWithIncludesAsync(int id,
                                            bool includeDeleted = false,
                                            CancellationToken ct = default,
                                            params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TEntity?> GetByIdIncludingDeletedAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
    {
        return await _dbSet.IgnoreQueryFilters().AnyAsync(expression, ct);
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
    {
        return await _dbSet.Where(expression).ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking().IgnoreQueryFilters();
        if (predicate is not null)
            query = query.Where(predicate);
        return await query.CountAsync(ct);
    }

    public async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    //public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    //    => await _gymDbContext.SaveChangesAsync(ct);

}
