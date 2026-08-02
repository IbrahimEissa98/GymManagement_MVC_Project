using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        => await _dbSet.AsNoTracking().ToListAsync(ct);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<TEntity?> GetByIdWithIncludesAsync(int id, CancellationToken ct = default,
                                            params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TEntity?> GetByIdIncludingDeletedAsync(int id, CancellationToken ct = default)
        => await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
        => await _dbSet.AnyAsync(expression, ct);

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
        => await _dbSet.Where(expression).ToListAsync(ct);

    public async Task AddAsync(TEntity entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void Remove(TEntity entity)
        => _dbSet.Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _gymDbContext.SaveChangesAsync(ct);

}
