using Microsoft.EntityFrameworkCore;

namespace DatabaseBenchmark
{
    public class SqlTestDbContext : TestDbContext
    {
        static SqlTestDbContext()
        {
        }
        public SqlTestDbContext(DbContextOptions<SqlTestDbContext> options) : base(options)
        {
        }
    }
}
