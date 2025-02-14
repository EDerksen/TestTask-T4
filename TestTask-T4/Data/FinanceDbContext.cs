using Microsoft.EntityFrameworkCore;
using TestTask_T4.Data.Extensions;
using TestTask_T4.Model;

namespace TestTask_T4.Data
{
    public class FinanceDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<FinancialTransaction> Transactions { get; set; }
        public DbSet<ClientBalanceSnapshot> BalanceSnapshots { get; set; }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinancialTransaction>()
                .HasOne(ft => ft.Client)
                .WithMany();

            modelBuilder.Entity<FinancialTransaction>()
                .HasOne(ft => ft.Client)
                .WithMany();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            this.SetCreatedAt();
            this.SetUpdatedAt();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.SetCreatedAt();
            this.SetUpdatedAt();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
