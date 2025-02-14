using Microsoft.EntityFrameworkCore;
using TestTask_T4.Model;

namespace TestTask_T4.Data
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<FinanceTransaction> Transactions { get; set; }


        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinanceTransaction>()
                .HasOne(ft => ft.Client)
                .WithMany();

            base.OnModelCreating(modelBuilder);
        }
    }
}
