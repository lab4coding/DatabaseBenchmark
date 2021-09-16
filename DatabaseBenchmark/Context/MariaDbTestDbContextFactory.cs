using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseBenchmark
{
    public class MariaDbTestDbContextFactory : IDesignTimeDbContextFactory<MariaDbTestDbContext>
    {
        private const string _connectionString = "server=localhost;database=db_benchmark;user=root;password=password;port=3307;";

        public MariaDbTestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MariaDbTestDbContext>();
            var serverVersion = ServerVersion.AutoDetect(_connectionString);
            //serverVersion = new MariaDbServerVersion("10.6.4");
            optionsBuilder.UseMySql(_connectionString, serverVersion);

            return new MariaDbTestDbContext(optionsBuilder.Options);
        }
    }
}
