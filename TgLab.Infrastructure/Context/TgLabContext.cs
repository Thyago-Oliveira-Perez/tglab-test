using Microsoft.EntityFrameworkCore;
using TgLab.Domain.Models;

namespace TgLab.Infrastructure.Context
{
    public partial class TgLabContext : DbContext
    {
        public TgLabContext(DbContextOptions<TgLabContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Bet> Bets { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfigurationsFromAssembly(typeof(TgLabContext).Assembly);
            base.OnModelCreating(model);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
