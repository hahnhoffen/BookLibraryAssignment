using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookLibrary.Infrastructure.Data.Factory
{
    public class RealDataBaseContextFactory : IDesignTimeDbContextFactory<RealDataBase>
    {
        public RealDataBase CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RealDataBase>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new RealDataBase(optionsBuilder.Options);
        }
    }
}

