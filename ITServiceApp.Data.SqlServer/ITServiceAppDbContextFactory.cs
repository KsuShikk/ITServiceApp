using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ITServiceApp.Data.SqlServer
{
    public class ITServiceAppDbContextFactory : IDesignTimeDbContextFactory<ITServiceAppDbContext>
    {
        public ITServiceAppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            return CreateDbContext(configuration);
        }

        public ITServiceAppDbContext CreateDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ITServiceAppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ITServiceAppDbContext(optionsBuilder.Options);
        }
    }
}