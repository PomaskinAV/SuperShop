using Microsoft.EntityFrameworkCore;
using MyShopBackend.Data;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace MyShopBackend.Repositories
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly AppDbContext _dbContext;

        public EFRepository(AppDbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();
        public virtual Task<TEntity> GetById(Guid Id, CancellationToken cancellationToken)
            => Entities.FirstAsync(it => it.Id == Id, cancellationToken);
        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync();
        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Entities.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
