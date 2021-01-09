 
using System;

namespace ChatApp.Domain
{
     
    public class User : UserTableEntity<Guid>
    {
        public string Username { get; set; }
        public string Name { get; set; } 
        public string Surname { get; set; }
        
        public string Phone { get; set; }
        public string Email { get; set; }
           
        public Guid IdentityUserID { get; set; }
         
    }
}
