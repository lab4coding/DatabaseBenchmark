using Microsoft.EntityFrameworkCore;

namespace DatabaseBenchmark
{
    public class TestDbContext: DbContext
    {
        protected TestDbContext(DbContextOptions options): base(options)
        {

        }

        public TestDbContext()
        {

        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var users = modelBuilder.Entity<User>();

            users.HasKey(e => e.Id);
        }
    }
}
