using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseBenchmark
{
    public class MySqlTestDbContextFactory : IDesignTimeDbContextFactory<MySqlTestDbContext>
    {
        private const string _connectionString = "server=localhost;database=db_benchmark;user=root;password=password";

        public MySqlTestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlTestDbContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));

            return new MySqlTestDbContext(optionsBuilder.Options);
        }
    }
}
