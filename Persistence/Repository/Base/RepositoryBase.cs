
using Domain.Entities.Base;
using Domain.Repositories.Base;
using Domain.Specifications.Base;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence.Repository.Base
{
    public class RepositoryBase<T, TId> : IRepositoryBase<T, TId> where T : class, IEntityBase<TId>
    {
        public RepositoryBase(AnnouncementContext context)
        {
            _context = context;
        }

        private readonly DbContext _context;

        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();

                return _entities;
            }
        }

        public async virtual Task<T> GetByIdAsync(TId id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task<bool> AnyAsync(Expression<Func<T,bool>> expression)
        {
            return await Entities.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await Entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }
        public async Task<T> AddAsync(T entity)
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (entity.Id == null || entity.Id.Equals(default(TId)))
            {
                Entities.Add(entity);
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteByIdAsync(TId id)
        {
            var entity = Entities.Find(id);
            Entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T t, TId id)
        {
            if (t == null)
                return null;
            T exist = await _entities.FindAsync(id);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
                await _context.SaveChangesAsync();
            }
            return exist;
        }


        public async virtual Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public IQueryable<T> Table => Entities;

        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true)
        {
            var query = disableTracking ? TableNoTracking : Table;

            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            var query = disableTracking ? TableNoTracking : Table;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T, TId>.GetQuery(Table, spec);
        }
    }
}
