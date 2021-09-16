using Microsoft.EntityFrameworkCore;

namespace DatabaseBenchmark
{
    public class PostgreSqlTestDbContext : TestDbContext
    {
        public PostgreSqlTestDbContext(DbContextOptions<PostgreSqlTestDbContext> options) : base(options)
        {
        }
    }
}
