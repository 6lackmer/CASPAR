using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public T? Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public T? Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
        {
            IQueryable<T> queryable = _context.Set<T>();

            if (includes != null)
            {
                foreach (var property in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(property);
                }
            }
            if (asNoTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable.Where(predicate).FirstOrDefault();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual IEnumerable<T> List()
        {
            return _context.Set<T>().ToList().AsEnumerable();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
        {
            IQueryable<T> queryable = _context.Set<T>();

            if (includes != null)
            {
                foreach (var property in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(property);
                }
            }
            if (asNoTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            return await queryable.Where(predicate).FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _context.Set<T>();

            if (includes != null)
            {
                foreach (var property in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(property);
                }
            }

            queryable = queryable.Where(predicate);

            if (orderBy != null)
            {
                queryable = queryable.OrderBy(orderBy);
            }
            return queryable.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _context.Set<T>();

            if (includes != null)
            {
                foreach (var property in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(property);
                }
            }

            queryable = queryable.Where(predicate);

            if (orderBy != null)
            {
                queryable = queryable.OrderBy(orderBy);
            }
            return await queryable.ToListAsync();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
