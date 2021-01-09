 
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ChatApp.Service
{
    public class OneSignalService
    {
        private readonly string _oneSignalApiUrl = "https://onesignal.com/api/v1/notifications";
        private readonly string _oneSignalAppId = "";
        private readonly string _oneSignalRestApiKey = "";

        public OneSignalService(string oneSignalAppId = "*******", string oneSignalRestApiKey = "*******")
        {
            _oneSignalAppId = oneSignalAppId;
            _oneSignalRestApiKey = oneSignalRestApiKey;
        }

        public APIResult<bool> SendToNotification(string title, string content, ICollection<string> userList, DateTime? sendDate = null)
        {

            HttpWebRequest request = WebRequest.Create(_oneSignalApiUrl) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + _oneSignalRestApiKey);
           
            object obj = new object();

            #region Filters 

            List<object> _filters = new List<object>();

            if (userList != null)
            {
                foreach (string userName in userList)
                {
                    _filters.Add(new
                    {
                        field = "tag",
                        key = "Username",
                        relation = "=",
                        value = userName
                    });
                }
            }

            #endregion

            if (sendDate == null)
            {
                obj = new
                {
                    app_id = _oneSignalAppId,
                    contents = new { en = content },
                    headings = new { en = title },
                    filters = _filters
                };
            }
            else
            {
                obj = new
                {
                    app_id = _oneSignalAppId,
                    contents = new { en = content },
                    headings = new { en = title },
                    filters = _filters,
                    send_after = sendDate.Value.ToUniversalTime().ToLongDateString() + " " + sendDate.Value.ToUniversalTime().ToLongTimeString()
                };
            }


            string param = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
                return new APIResult<bool> { IsSuccess = true };
            }
            catch (WebException ex)
            {
                responseContent = ex.ToString();
                return new APIResult<bool> { Message = responseContent };
            }

        }

        public APIResult<string> GetNotifications()
        {

            HttpWebRequest request = WebRequest.Create(_oneSignalApiUrl + "?app_id=" + _oneSignalAppId) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + _oneSignalRestApiKey);

            string responseContent = null;

            try
            {

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
                return new APIResult<string> { Data = responseContent, IsSuccess = true };
            }
            catch (WebException ex)
            {
                responseContent = ex.ToString();
                return new APIResult<string> { Message = responseContent };
            }

        }

        public APIResult<string> GetNotification(string notificationID)
        {

            HttpWebRequest request = WebRequest.Create(_oneSignalApiUrl + "/" + notificationID + "?app_id=" + _oneSignalAppId) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Basic " + _oneSignalRestApiKey);
             
            string responseContent = null;

            try
            {
              
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
                return new APIResult<string> { Data = responseContent, IsSuccess = true };
            }
            catch (WebException ex)
            {
                responseContent = ex.ToString();
                return new APIResult<string> { Message = responseContent };
            }

        }

    }
}
