using ChatApp.Data;
using ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public class EntityUnitofWork<Y> : IDisposable
      where Y : struct
    {
        private TulparDbContext con;
        public EntityUnitofWork(TulparDbContext _con)
        {
            con = _con; 
        }
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IRepository<T, Y> Repository<T>() where T : Entity<Y>
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T, Y>;
            }
            IRepository<T, Y> repository = new EntityRepository<T, Y>(con);
            repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<ICollection<T>> ListSqlQuery<T>(string sql) where T : Entity<Y>
        {
            try
            {
                var returnList = new List<T>();
                await Task.Run(() => {
                    returnList = con.Query<T>().FromSql(sql).ToList();

                });
                return returnList;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }


        public async Task<decimal> SqlQueryNumeric(string sql) 
        {
            try
            {
                decimal returnModel = 0;
                await Task.Run(() => {
                    returnModel = con.Database.ExecuteSqlCommand(sql);
                });
                return returnModel;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await con.SaveChangesAsync();
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    con.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
