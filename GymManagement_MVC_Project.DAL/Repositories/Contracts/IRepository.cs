using GymManagement_MVC_Project.DAL.Models;
using System.Linq.Expressions;

namespace GymManagement_MVC_Project.DAL.Repositories.Contracts;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TEntity?> GetByIdIncludingDeletedAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
