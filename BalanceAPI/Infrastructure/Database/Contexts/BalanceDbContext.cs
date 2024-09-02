using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Database.Contexts
{
    public class BalanceDbContext : DbContext
    {
        public BalanceDbContext(DbContextOptions<BalanceDbContext> options) : base(options)
        {
        }

        public DbSet<BalanceEntity> Balances { get; set; }
        public DbSet<OperationEntity> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<BalanceEntity>()
                .HasKey(x => x.Date);

            base.OnModelCreating(modelBuilder);
        }
    }
}
