using ChatApp.API.Helpers;
using ChatApp.API.Models;
using ChatApp.Const;
using ChatApp.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.API.Controllers
{
    /// <summary>
    /// Authentication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        private readonly IUserService _userService;

        /// <summary>
        /// Authentication Controller
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public AuthController(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager,
            RoleManager<AppIdentityRole> roleManager,
            IConfiguration configuration,
            IUserService userService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="StringModel"></param>
        /// <returns></returns>
        [Route("Token"), HttpPost] 
        [ProducesResponseType(typeof(TokenDto), 200)] 
        public async Task<IActionResult> Token(StringModel<LoginDto> StringModel)
        {
            try
            {


                LoginDto model = AESEncryptDecrypt<LoginDto>.DecryptStringAES(StringModel.Model);

                byte[] ecodedString = Convert.FromBase64String(model.Password);
                string password = Encoding.UTF8.GetString(ecodedString);

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, password, false, false);
                if (result.Succeeded)
                {
                    AppIdentityUser appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
                    if (appUser == null)
                    {
                        return Ok(new APIResult<Guid> { Message = Messages.NoRecord, IsSuccess = false });
                    }

                    IList<string> appUserRoles;
                    appUserRoles = await _userManager.GetRolesAsync(appUser);

                    Service.DTOs.UserCardDto user = await _userService.GetUserByIdentityID(Guid.Parse(appUser.Id));
  
                    string token = TokenBuilder.CreateJsonWebToken(appUser.UserName, appUserRoles, "https://www.tulparyazilim.com.tr", "https://www.tulparyazilim.com.tr", Guid.NewGuid(), DateTime.UtcNow.AddDays(30));

                    var data = new TokenDto
                    {
                        ValidTo = DateTime.UtcNow.AddDays(30),
                        Value = token,
                        Roles = string.Join(',', appUserRoles),
                        Username = appUser.UserName,
                        Email = appUser.Email
                    };

                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    string base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                    string encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data);

                    return Ok(new APIResult<string>
                    {
                        Message = Messages.Ok,
                        Data = encryptredData,
                        IsSuccess = true
                    });
                }
                return Ok(new APIResult<Guid> { Message = Messages.NoRecord, IsSuccess = false });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<Guid> { Message = ex.ToString(), IsSuccess = false  });
            }
        }
         
    }


}