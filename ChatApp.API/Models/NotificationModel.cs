using System;
using System.Collections.Generic;

namespace ChatApp.API
{
    public class NotificationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public DateTime? SendDate { get; set; }
        public ICollection<string> Users { get; set; }
        public NotificationModel()
        {
            this.Users = new List<string>();
        } 
    }
}
