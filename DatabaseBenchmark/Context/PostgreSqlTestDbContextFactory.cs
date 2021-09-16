using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseBenchmark
{
    public class PostgreSqlTestDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlTestDbContext>
    {
        public PostgreSqlTestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlTestDbContext>();
            optionsBuilder.UseNpgsql("Host=localhos;Database=db_benchmark;Username=postgres;Password=password;Port=5433");

            return new PostgreSqlTestDbContext(optionsBuilder.Options);
        }
    }
}
