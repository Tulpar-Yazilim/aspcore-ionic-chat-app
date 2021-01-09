using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API
{
     

    public class WebAPITulparDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        public WebAPITulparDbContext(DbContextOptions<WebAPITulparDbContext> options) : base(options) {
        }
         
    }
}
