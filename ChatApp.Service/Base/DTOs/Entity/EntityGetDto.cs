using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Service
{
    public class EntityBaseGetDto<Y>
    {
        [Required]
        public Y ID { get; set; } 
    }
    public class EntityGetDto<Y> : EntityBaseGetDto<Y>
    {
        public virtual string Code { get; set; }
        //public virtual UserLeanDto CreateBy { get; set; }
        public virtual DateTime CreateDT { get; set; }
        //public virtual UserLeanDto UpdateBy { get; set; }
        public virtual DateTime? UpdateDT { get; set; }
    }

    public class EntityGetLeanDto<Y> : EntityBaseGetDto<Y>
    {
        public virtual string Code { get; set; } 
    }
}
