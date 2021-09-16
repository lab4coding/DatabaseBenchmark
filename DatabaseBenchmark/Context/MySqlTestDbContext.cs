using Microsoft.EntityFrameworkCore;

namespace DatabaseBenchmark
{
    public class MySqlTestDbContext : TestDbContext
    {
        public MySqlTestDbContext(DbContextOptions<MySqlTestDbContext> options) : base(options)
        {
        }
    }
}
