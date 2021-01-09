using AutoMapper;
using ChatApp.Domain;
using ChatApp.Service.DTOs;

namespace ChatApp.Service.AutoMapper
{
    public class AutoMapperConfig<Y>
       where Y : struct
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            { 
                #region User Mapper
                x.CreateMap<UserAddDto, User>();
                x.CreateMap<UserUpdateDto, User>();
                x.CreateMap<User, UserCardDto>();
                x.CreateMap<User, UserAddDto>();
                x.CreateMap<User, UserUpdateDto>();
                #endregion
                 });
        }
    }

}
