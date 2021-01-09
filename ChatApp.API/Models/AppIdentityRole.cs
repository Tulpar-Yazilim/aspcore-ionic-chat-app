using Microsoft.AspNetCore.Identity;

namespace ChatApp.API
{
    public class AppIdentityRole : IdentityRole
    {
        public int CacheMinute { get; set; } 
    }
}
