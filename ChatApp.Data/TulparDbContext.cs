using ChatApp.Const;
using ChatApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class TulparDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region Database Settings
            if (Messages.RunTimeProjectType == DevelopmentMode.Local)
            {
                optionsBuilder.UseNpgsql(Messages.DatabaseLocalConnectionStr);
            }
            else if (Messages.RunTimeProjectType == DevelopmentMode.Development)
            {
                optionsBuilder.UseNpgsql(Messages.DatabaseTestConnectionStr);
            }
            else if (Messages.RunTimeProjectType == DevelopmentMode.Production)
            {
                optionsBuilder.UseNpgsql(Messages.DatabaseProdConnectionStr);
            }
            #endregion 
        }

        public virtual DbSet<User> Users { get; set; } 
    }
}
