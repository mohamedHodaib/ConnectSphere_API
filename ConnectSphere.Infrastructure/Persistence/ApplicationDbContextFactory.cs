using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ConnectSphere.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build configuration to read the connection string
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                //To obtain UserSecretsId
                .AddUserSecrets<Program>
                .Build();


            // Create the DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("ConnectionString");

            optionsBuilder.UseSqlServer(connectionString);

            // Return a new instance of the DbContext
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
