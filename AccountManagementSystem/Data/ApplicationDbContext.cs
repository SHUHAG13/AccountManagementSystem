using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniAccountApi.Models;

namespace MiniAccountApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherEntry> VoucherEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChartOfAccount>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId);

        }

    }
}