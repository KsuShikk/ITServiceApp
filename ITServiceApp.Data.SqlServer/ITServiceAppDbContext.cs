using Microsoft.EntityFrameworkCore;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.SqlServer
{
    public class ITServiceAppDbContext : DbContext
    {
        public ITServiceAppDbContext(DbContextOptions<ITServiceAppDbContext> options) : base(options) { }

        public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;
        public DbSet<Engineer> Engineers { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Если хочешь сохранить Client как owned entity, можно раскомментировать:
            // modelBuilder.Entity<ServiceRequest>().OwnsOne(r => r.Client);
        }
    }
}

