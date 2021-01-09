using ChatApp.Service.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.Models
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(4)]
        public string Password { get; set; } 
    }

    public class RegisterDto : UserAddDto
    {
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password), MinLength(6)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } 
    }

    public class TokenDto
    {
        public DateTime ValidTo { get; set; }
        public string Value { get; set; }
        public string Roles { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } 
    }
}
