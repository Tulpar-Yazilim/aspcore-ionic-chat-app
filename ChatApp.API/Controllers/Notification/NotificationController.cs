
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Service;
using System;
using System.Text;

namespace ChatApp.API.Controllers
{
    /// <summary>
    /// Notification Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {

        public OneSignalService oneSignalService; 

        public NotificationController()
        {
            oneSignalService = new OneSignalService();
        }

        /// <summary>
        /// Get Notification List
        /// </summary>
        /// <returns></returns>
        [Route("GetList"), HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult GetList()
        {
            try
            {

                APIResult<string> notificationResult = oneSignalService.GetNotifications();
                if (!notificationResult.IsSuccess)
                {
                    return Ok(notificationResult);
                }
                 
                string base64Data = Convert.ToBase64String(Encoding.Default.GetBytes(notificationResult.Data));
                string encryptredData = AESEncryptDecrypt<string>.EncryptStringAES(base64Data);
                return Ok(new APIResult<string> { Data = encryptredData, IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<string> { Message = ex.ToString(), IsSuccess = false });
            }

        }

         
        [Route("Send"), HttpPost] 
        public IActionResult Send(StringModel<NotificationModel> StringModel)
        {
            try
            {
                NotificationModel notificationModel = AESEncryptDecrypt<NotificationModel>.DecryptStringAES(StringModel.Model);
                var sendResult = oneSignalService.SendToNotification(notificationModel.Title, notificationModel.Message, notificationModel.Users, notificationModel.SendDate);
                if (!sendResult.IsSuccess)
                {
                    return Ok(sendResult);
                }
                return Ok(new APIResult<string> { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new APIResult<string> { Message = ex.ToString(), IsSuccess = false });
            }
        }

    }

}
