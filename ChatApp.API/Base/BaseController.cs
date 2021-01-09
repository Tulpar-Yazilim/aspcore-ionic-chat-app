using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Service;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BaseController<A, U, G, Y, S> : ControllerBase
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityBaseGetDto<Y>
        where S : ICRUDService<A, U, G, Y>
    {

        protected S service;
        public BaseController(S _service
            )
        {
            service = _service;
        }


        /// <summary>
        /// Add
        /// </summary>
        /// <param name="StringModel"></param>
        /// <returns></returns>
        [Route("Add"), HttpPost] 
        public virtual async Task<IActionResult> Add(StringModel<A> StringModel)
        {
            try
            {
                A addDto = AESEncryptDecrypt<A>.DecryptStringAES(StringModel.Model);
                APIResult<Guid> result = await service.Add(addDto, GetUserId<Y>());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<Guid> { IsSuccess = false });
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="StringModel"></param>
        /// <returns></returns>
        [Route("Update"), HttpPut]
        public virtual async Task<IActionResult> Update(StringModel<U> StringModel)
        {
            try
            {
                U updateDto = AESEncryptDecrypt<U>.DecryptStringAES(StringModel.Model);
                APIResult<Guid> result = await service.Update(updateDto, GetUserId<Y>());
                return Ok(result);
            }
            catch (Exception)
            {

                return Ok(new APIResult<Guid> { IsSuccess = false });
            }
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Delete"), HttpDelete]
        public virtual async Task<IActionResult> Delete(Y id)
        {
            try
            {
                APIResult<Guid> result = await service.Delete(id, GetUserId<Y>());
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(new APIResult<Guid> { IsSuccess = false });
            }
        }

        /// <summary>
        /// Get Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [Route("Get"), HttpGet]
        public virtual async Task<IActionResult> GetById(Y id)
        {
            try
            {
                var data = await service.GetByID(id);
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                var encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data); 
                APIResult<string> result = new APIResult<string> { Data = encryptredData, IsSuccess = true };
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(new APIResult<Guid> { IsSuccess = false });
            }
        }

        /// <summary>
        /// GetUserId With HttpContext
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <returns></returns>
        protected Y GetUserId<Y>()
             where Y : struct
        {
            string data = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            return (Y)TypeDescriptor.GetConverter(typeof(Y)).ConvertFromInvariantString(Guid.Parse(data).ToString());
        }

        /// <summary>
        /// GetIdentityUserId With HttpContext
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <returns></returns>
        protected Y GetIdentityUserId<Y>()
             where Y : struct
        {
            string data = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "IdentityUserId").Value;
            return (Y)TypeDescriptor.GetConverter(typeof(Y)).ConvertFromInvariantString(Guid.Parse(data).ToString());
        }


    }
}
