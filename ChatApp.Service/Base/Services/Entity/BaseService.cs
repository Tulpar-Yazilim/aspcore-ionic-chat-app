using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp.Const;
using ChatApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public interface IBaseService<A, U, G, D, Y> : ICRUDService<A, U, G, Y>
        where Y : struct
        where D : Entity<Y>
        where U : EntityUpdateDto<Y>
        where G : EntityBaseGetDto<Y>
    {
        D AddMapping(A entity);
    }

    public class BaseService<A, U, G, D, Y> : IBaseService<A, U, G, D, Y>
        where Y : struct
        where D : Entity<Y>
        where U : EntityUpdateDto<Y>
        where G : EntityBaseGetDto<Y>

    {
        #region Common
        protected EntityUnitofWork<Y> _uow;
        public BaseService(EntityUnitofWork<Y> uow)
        {
            _uow = uow;
        }
        public virtual D AddMapping(A model)
        {
            D entity = Mapper.Map<D>(model);
            return entity;
        }
        #endregion

        #region Add
        /// <summary>
        /// New Recoard 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        public virtual async Task<APIResult<Guid>> Add(A model, Y? userID, bool isCommit = true)
        {
            try
            {
                D entity = AddMapping(model);

                Y pkType = default(Y);

                if (pkType is Guid)
                {
                    if ((entity.ID as Guid?).IsNullOrEmpty())
                    {
                        entity.ID = Guid.NewGuid();
                    }
                }

                if (entity is ITableEntity<Y> || entity is UserTableEntity<Y>)
                {
                    if (userID != null && entity is ITableEntity<Y>)
                    {
                        (entity as ITableEntity<Y>).CreateByID = Guid.Parse(userID.ToString());
                    }
                    if (entity is ITableEntity<Y>)
                    {
                        (entity as ITableEntity<Y>).CreateDT = DateTime.Now;
                    }
                    if (entity is UserTableEntity<Y>)
                    {
                        (entity as UserTableEntity<Y>).CreateDT = DateTime.Now;
                    }
                }
                 
                _uow.Repository<D>().Add(entity);

                if (isCommit)
                {
                    await _uow.SaveChangesAsync();
                }

                return new APIResult<Guid> { Data = entity.ID, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new APIResult<Guid>() { Message = ex.ToString() };
            }
        }

        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID"></param>
        /// <param name="isCommit"></param>
        /// <param name="checkAuthorize"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public virtual async Task<APIResult<Guid>> Update(U model, Y? userID = null, bool isCommit = true, bool checkAuthorize = false, bool isDeleted = false)
        {
            try
            {
                Guid modelID = Guid.Parse(model.ID.ToString());
                D entity = await _uow.Repository<D>().QueryGetBy(x => x.ID == modelID, isDeleted);
                if (entity == null)
                {
                    return new APIResult<Guid> { Data = modelID, Message = Messages.NoRecord };
                }

                if (entity is ITableEntity<Y>)
                {
                    ///Access Control
                    if (userID != null && checkAuthorize)
                    {
                        if (!(entity as ITableEntity<Y>).CreateByID.Equals(userID.Value))
                        {
                            return new APIResult<Guid> { Data = modelID, Message = Messages.Unauthorized };
                        }
                    }
                    (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                    (entity as ITableEntity<Y>).UpdateByID = Guid.Parse(userID.Value.ToString());
                }
                D mappedDto = Mapper.Map(model, entity);
                if (isCommit)
                {
                    await _uow.SaveChangesAsync();
                }

                return new APIResult<Guid> { Data = entity.ID, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new APIResult<Guid> { Message = ex.ToString() };
            }
        }
        #endregion

        #region Delete
        public virtual async Task<APIResult<Guid>> Delete(Y id, Y? userID = null, bool isCommit = true, bool checkAuthorize = false)
        {
            try
            {
                Guid modelID = Guid.Parse(id.ToString());
                D entity = await _uow.Repository<D>().QueryGetBy(x => x.ID == modelID);
                if (entity == null)
                {
                    return new APIResult<Guid>() { Data = modelID, Message = Messages.NoRecord };
                }

                if (entity is ITableEntity<Y>)
                {
                    //Access Control
                    if (userID != null && checkAuthorize)
                    {
                        if (!(entity as ITableEntity<Y>).CreateByID.Equals(userID.Value))
                        {
                            return new APIResult<Guid>() { Message = Messages.Unauthorized };
                        }
                    }
                     (entity as ITableEntity<Y>).UpdateDT = DateTime.Now;
                    (entity as ITableEntity<Y>).UpdateByID = Guid.Parse(userID.Value.ToString());
                }
                entity.IsDeleted = true;
                if (isCommit)
                {
                    await _uow.SaveChangesAsync();
                }

                return new APIResult<Guid>() { Data = modelID, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new APIResult<Guid>() { Message = ex.ToString() };
            }
        }
        #endregion

        #region Get
        public virtual async Task<G> GetByID(Y id, Y? userId = null, bool isDeleted = false)
        {
            try
            {
                IQueryable<D> query = _uow.Repository<D>().Query(isDeleted).Where(Predicate.Equal<D, Y>("ID", id));

                if (userId != null)
                    query = query.Equal("CreateByID", userId.Value);
                 
                G data = await query.ProjectTo<G>().FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion



    }
}
