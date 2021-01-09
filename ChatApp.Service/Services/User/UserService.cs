using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatApp.Const;
using ChatApp.Domain;
using ChatApp.Service.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Service
{
    public interface IUserService : IBaseService<UserAddDto, UserUpdateDto, UserCardDto, User, Guid>
    {
        Task<APIResult<Guid>> Register(UserAddDto model, Guid IdentityUserId);
        Task<List<UserCardDto>> GetList();
        Task<UserCardDto> GetUserByIdentityID(Guid IdentityID); 
    }

    public class UserService : BaseService<UserAddDto, UserUpdateDto, UserCardDto, User, Guid>, IUserService
    {
        public UserService(EntityUnitofWork<Guid> _uow) : base(_uow)
        {
        }

        public async Task<UserCardDto> GetUserByIdentityID(Guid IdentityID)
        {
            try
            {
                return await _uow.Repository<User>().Query().Where(x => x.IdentityUserID == IdentityID).ProjectTo<UserCardDto>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new UserCardDto();
            }
        }
         
        public async Task<List<UserCardDto>> GetList()
        {
            var data = await _uow.Repository<User>().Query().ProjectTo<UserCardDto>().ToListAsync();
            return data;
        }

        public async Task<APIResult<Guid>> Register(UserAddDto model, Guid IdentityUserId)
        {
            try
            {
                User entity = Mapper.Map<User>(model);
                entity.ID = Guid.NewGuid();
                entity.IdentityUserID = IdentityUserId; 
                entity.CreateDT = DateTime.Now; 

                _uow.Repository<User>().Add(entity);
                await _uow.SaveChangesAsync();
                return new APIResult<Guid> { Data = entity.ID, IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new APIResult<Guid> { Message = Messages.Error, IsSuccess = false };
            }
        } 
    }
}
