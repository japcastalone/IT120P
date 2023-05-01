using IT120P.Models;
using Microsoft.EntityFrameworkCore;

namespace IT120P.Data
{
    public class DeviceDbContext : DbContext
    {
        public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options)
        {
        }

        public DbSet<Devices> Devices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Devices>().HasKey(d => d.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
