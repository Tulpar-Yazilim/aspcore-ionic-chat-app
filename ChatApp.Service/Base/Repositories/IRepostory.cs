using ChatApp.Data;
using ChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public interface IRepository<T, Y>
        where T : Entity<Y>
        where Y : struct
    {
        TulparDbContext Context { get; set; }
        Task<APIResult<T>> GetByID(Y id, bool isDeleted = false);
        IQueryable<T> Get(bool isDeleted = false);
        IQueryable<T> Query(bool isDeleted = false, bool allRecord = false);
        Task<T> QueryGetBy(Expression<Func<T, bool>> expression, bool isDeleted = false, bool allRecords = false);
        Task<List<T>> QueryGetByList(Expression<Func<T, bool>> expression, bool isDeleted = false);
        void Add(T entity);
        void Remove(T entity); 
    }
}
