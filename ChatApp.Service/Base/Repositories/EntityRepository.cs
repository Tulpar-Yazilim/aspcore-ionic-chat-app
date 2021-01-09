using ChatApp.Const;
using ChatApp.Data;
using ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public class EntityRepository<T, Y> : IRepository<T, Y>
        where T : Entity<Y>
        where Y : struct
    {
        private TulparDbContext con;
        public TulparDbContext Context
        {
            get => con;
            set => con = value;
        }

        public EntityRepository(TulparDbContext context)
        {
            con = context;
        }

        public virtual async Task<APIResult<T>> GetByID(Y id, bool isDeleted = false)
        {
            try
            {
                return new APIResult<T>
                {
                    Data = await con.Set<T>().
                                     Where(Predicate.Equal<T, Y>("ID", id)).
                                     Where(x => x.IsDeleted == isDeleted).
                                     FirstOrDefaultAsync(),
                    Message = Messages.Ok
                };
            }
            catch (Exception)
            {
                return new APIResult<T> { Message = Messages.Error };
            }

        }

        public IQueryable<T> Get(bool isDeleted = false)
        {
            return con.Set<T>().Where(x => x.IsDeleted == isDeleted).AsQueryable();
        }

        public virtual async Task<T> QueryGetBy(Expression<Func<T, bool>> expression, bool isDeleted = false, bool allRecords = false)
        {
            if (allRecords)
            {
                return await con.Set<T>().
                        Where(expression).
                        FirstOrDefaultAsync();
            }
            else
            {
                return await con.Set<T>().
                        Where(expression).
                        Where(x => x.IsDeleted == isDeleted).
                        FirstOrDefaultAsync();
            }

        }
        public virtual async Task<List<T>> QueryGetByList(Expression<Func<T, bool>> expression, bool isDeleted = false)
        {
            return await con.Set<T>().
              Where(expression).
              Where(x => x.IsDeleted == isDeleted).
              ToListAsync();
        }
        
        public IQueryable<T> Query(bool isDeleted = false, bool allRecord = false)
        {
            if (allRecord)
            {
                return con.Set<T>().AsNoTracking();
            }
            else
            {
                return con.Set<T>().AsNoTracking().Where(x => x.IsDeleted == isDeleted);
            }

        }
        public virtual void Add(T entity)
        {
            con.Set<T>().Add(entity);
        }
        public virtual void Remove(T entity)
        {
            con.Set<T>().Remove(entity);
        }

        
    }
}
