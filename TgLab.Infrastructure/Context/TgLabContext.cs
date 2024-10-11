using Microsoft.EntityFrameworkCore;
using TgLab.Domain.Models;

namespace TgLab.Infrastructure.Context
{
    public partial class TgLabContext : DbContext
    {
        public TgLabContext() { }

        public TgLabContext(DbContextOptions<TgLabContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Bet> Bets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(k => k.Id);
                entity.HasIndex(k => k.Email).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
