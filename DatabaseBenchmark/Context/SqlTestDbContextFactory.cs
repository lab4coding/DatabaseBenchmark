using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseBenchmark
{
    public class SqlTestDbContextFactory : IDesignTimeDbContextFactory<SqlTestDbContext>
    {
        public SqlTestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlTestDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;AttachDbFilename=db_benchmark.mdf;Initial Catalog=db-benchmark;Integrated Security=True;MultipleActiveResultSets=True");

            return new SqlTestDbContext(optionsBuilder.Options);
        }
    }
}
