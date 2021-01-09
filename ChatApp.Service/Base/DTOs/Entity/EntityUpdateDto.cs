using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Service
{
    public class EntityUpdateDto<Y>
    {
        [Required]
        public Y ID { get; set; } 
    }
}
