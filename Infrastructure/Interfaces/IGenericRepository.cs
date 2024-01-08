using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T? Get(int id);

        T? Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null);

        IEnumerable<T> List();

        Task<T?> GetAsync(int id);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        void Update(T entity);
    }
}
