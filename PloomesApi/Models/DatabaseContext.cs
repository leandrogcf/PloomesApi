using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PloomesApi.Models
{
    public class DatabaseContext : DbContext
    {
        private IConfiguration configuration;
        public DbSet<User> Users { get; set; }
        public DbSet<Case> Cases { get; set; }

        public DatabaseContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(configuration.GetConnectionString("PloomesDatabase"));
    }
}
