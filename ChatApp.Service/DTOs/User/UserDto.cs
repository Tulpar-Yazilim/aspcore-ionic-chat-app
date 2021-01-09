using System;
using System.Collections.Generic;

namespace ChatApp.Service.DTOs
{
    public class UserAddDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        public List<string> Roles { get; set; }
        public UserAddDto()
        {
            this.Roles = new List<string>();
        } 
    }

    public class UserUpdateDto : EntityUpdateDto<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        public string IdentityUserID { get; set; }
        public List<string> Roles { get; set; }
        public UserUpdateDto()
        {
            this.Roles = new List<string>();
        } 
    }

    public class UserCardDto : EntityGetDto<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } 
        public string IdentityUserID { get; set; }
        public List<string> Roles { get; set; }
        public UserCardDto()
        {
            this.Roles = new List<string>();
        }
    }


}
