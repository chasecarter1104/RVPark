using ApplicationCore.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        T GetByID(int id);
        T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);
        IEnumerable<T> List();
        Task<IEnumerable<T>> ListAsync();
        // Add entity
        void Add(T entity);
        // Delete entity
        void Delete(T entity);
        // Delete multiple entities
        void Delete(IEnumerable<T> entities);
        // Update entity
        void Update(T entity);
        IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null);
    }
}
