using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CloudDrive.Data.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        T? Find(int id);
        T? FirstOrDefault(Expression<Func<T, bool>> whereCondition);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Edit(T entity);
        void EditRange(List<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        int Save();
        Task<int> SaveAsync();
    }
}
