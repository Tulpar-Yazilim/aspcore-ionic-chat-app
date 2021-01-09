
using ChatApp.API.Models;
using ChatApp.Const;
using ChatApp.Service;
using ChatApp.Service.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.API.Controllers
{
    /// <summary>
    /// Users
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : BaseController<UserAddDto, UserUpdateDto, UserCardDto, Guid, IUserService>
    {

        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        /// <summary>
        /// User Controller
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="_service"></param>
        public UserController(
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager,
            IUserService _service
            ) : base(_service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// User Register
        /// </summary> 
        /// <param name="StringModel"></param> 
        /// <returns></returns>
        [Route("Register"), HttpPost] 
        public async Task<IActionResult> Register(StringModel<RegisterDto> StringModel)
        {
            try
            {

                RegisterDto model = AESEncryptDecrypt<RegisterDto>.DecryptStringAES(StringModel.Model);

                byte[] ecodedString = Convert.FromBase64String(model.Password);
                string password = Encoding.UTF8.GetString(ecodedString);

                AppIdentityUser identityUser = new AppIdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.Phone
                };

                IdentityResult result = await _userManager.CreateAsync(identityUser, password);

                if (result.Succeeded)
                {

                    if (model.Roles != null && model.Roles.Any())
                    {
                        IdentityResult rolesResult = await _userManager.AddToRolesAsync(identityUser, model.Roles);
                    }

                    APIResult<Guid> userResult = await service.Register(model, Guid.Parse(identityUser.Id));
                    if (userResult.IsSuccess)
                    {
                        return Ok(userResult);
                    }
                    else
                    {
                        await _userManager.DeleteAsync(identityUser);
                        return Ok(userResult);
                    }
                }
                else
                {
                    return Ok(new APIResult<List<string>> { Message = Messages.Error, Data = result.Errors.Select(x => x.Description).ToList(), IsSuccess = false });
                }

            }
            catch (Exception ex)
            {
                return Ok(new APIResult<List<string>> { Message = Messages.Error, Data = new List<string> { ex.ToString() }, IsSuccess = false });
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="StringModel"></param>
        /// <returns></returns> 
        public override async Task<IActionResult> Update(StringModel<UserUpdateDto> StringModel)
        {
            try
            {

                UserUpdateDto userUpdateDto = AESEncryptDecrypt<UserUpdateDto>.DecryptStringAES(StringModel.Model);

                APIResult<Guid> userUpdateResult = await service.Update(userUpdateDto, GetUserId<Guid>());
                if (userUpdateResult.Message != Messages.Ok)
                {
                    return Ok(userUpdateResult);
                }

                AppIdentityUser identityUser = await _userManager.FindByIdAsync(userUpdateDto.IdentityUserID);
                IList<string> identityUserRoles = await _userManager.GetRolesAsync(identityUser); 

                IdentityResult removeRolesResult = await _userManager.RemoveFromRolesAsync(identityUser, identityUserRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return Ok(new APIResult<List<string>> { Message = Messages.Error, Data = removeRolesResult.Errors.Select(x => x.Description).ToList(), IsSuccess = false });
                }
                IdentityResult addRolesResult = await _userManager.AddToRolesAsync(identityUser, userUpdateDto.Roles);
                if (!addRolesResult.Succeeded)
                {
                    return Ok(new APIResult<List<string>> { Message = Messages.Error, Data = addRolesResult.Errors.Select(x => x.Description).ToList(), IsSuccess = false });
                }

                return Ok(userUpdateResult);

            }
            catch (Exception ex)
            {
                return Ok(new APIResult<List<string>> { Data = new List<string> { ex.ToString() }, Message = Messages.Error, IsSuccess = false });
            }

        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserCardDto), 200)]
        public override async Task<IActionResult> GetById(Guid id)
        {
            try
            {

                UserCardDto user = await service.GetByID(id);
                if (user == null)
                {
                    return Ok(new APIResult<UserCardDto> { Message = Messages.NoRecord, IsSuccess = false });
                }

                AppIdentityUser identityUser = await _userManager.FindByIdAsync(user.IdentityUserID);
                IList<string> identityUserRoles = await _userManager.GetRolesAsync(identityUser); 
                user.Roles = identityUserRoles.ToList();

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                string base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                string encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data);

                return Ok(new APIResult<string> { Data = encryptredData, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<string> { Message = ex.ToString(), IsSuccess = false });
            }
        }

        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<UserCardDto>), 200)] 
        [Route("GetList"), HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                List<UserCardDto> data = await service.GetList();
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                string base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                string encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data);
                return Ok(new APIResult<string> { Data = encryptredData, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<string> { Message = ex.ToString(), IsSuccess = false });
            }

        }

        /// <summary>
        /// Get Role List
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<AppIdentityRole>), 200)]
        [Route("GetRoleList"), HttpGet]
        public IActionResult GetRoleList()
        {
            try
            {
                IQueryable<AppIdentityRole> data = _roleManager.Roles;
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                string base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                string encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data);
                return Ok(new APIResult<string> { Data = encryptredData, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<string> { Message = ex.ToString(), IsSuccess = false });
            }
        }

    }

}
