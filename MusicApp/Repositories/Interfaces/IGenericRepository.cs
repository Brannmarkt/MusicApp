using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace MusicApp.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        void Attach(T entity);
        void Detach(T entity);
    }
}
