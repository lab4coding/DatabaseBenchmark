using Microsoft.EntityFrameworkCore;

namespace DatabaseBenchmark
{
    public class MariaDbTestDbContext: TestDbContext
    {
        public MariaDbTestDbContext(DbContextOptions<MariaDbTestDbContext> options) : base(options)
        {
        }
    }
}
