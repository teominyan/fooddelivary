using Duende.IdentityServer.EntityFramework.Options;
using fooddelivary.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using fooddelivary.Shared.Domain;
using Microsoft.AspNetCore.Identity;

namespace fooddelivary.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(o => o.UserId);

            builder.Entity<Order>()
                .HasOne<Rider>()
                .WithMany()
                .HasForeignKey(o => o.RiderId);

            builder.Entity<Order>()
                .HasOne<Shop>()
                .WithMany()
                .HasForeignKey(o => o.ShopId);

            builder.Entity<Product>()
                .HasOne<Shop>()
                .WithMany()
                .HasForeignKey(p => p.ApplicationUserId);
        }

    }
}
