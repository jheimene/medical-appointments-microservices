using Microsoft.EntityFrameworkCore;
using Security.ApiGateway.Yarp.Models;

namespace Security.ApiGateway.Yarp.Data
{
    public class YarpConfigDbContext(DbContextOptions<YarpConfigDbContext> options) : DbContext(options)
    {
        public DbSet<ProxyCluster> Clusters => Set<ProxyCluster>();
        public DbSet<ProxyDestionation> Destinations => Set<ProxyDestionation>();
        public DbSet<ProxyRoute> Routes => Set<ProxyRoute>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProxyCluster>(e =>
            {
                e.ToTable("ProxyClusters", schema: "Yarp");
                e.HasKey(x => x.Id);
                e.Property(x => x.ClusterId).HasMaxLength(60).IsRequired();
                e.Property(x => x.LoadBalancingPolicy).HasMaxLength(50);
                e.HasIndex(x => x.ClusterId).IsUnique();
            });

            modelBuilder.Entity<ProxyDestionation>(e =>
            {
                e.ToTable("ProxyDestinations");
                e.HasKey(x => x.Id);
                e.Property(x => x.DestinationId).HasMaxLength(60).IsRequired();
                e.Property(x => x.Address).HasMaxLength(255).IsRequired();
                e.HasOne(x => x.Cluster)
                 .WithMany(c => c.Destinations)
                 .HasForeignKey(x => x.ClusterId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(x => new { x.ClusterId, x.DestinationId }).IsUnique();
            });

            modelBuilder.Entity<ProxyRoute>(e =>
            {
                e.ToTable("Route", schema: "Yarp");
                e.HasKey(x => x.Id);
                e.Property(x => x.RouteId).HasMaxLength(60).IsRequired();
                e.Property(x => x.ClusterId).HasMaxLength(60).IsRequired();
                e.Property(x => x.PathPattern).HasMaxLength(100).IsRequired();
                e.Property(x => x.RemovePrefix).HasMaxLength(50);
                e.Property(x => x.AuthorizationPolicy).HasMaxLength(100);
                e.Property(x => x.RateLimitPolicy).HasMaxLength(100);
                e.HasIndex(x => x.RouteId).IsUnique();
            });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(255)
                .AreUnicode(false)
                .HaveConversion<string>();

            configurationBuilder.Properties<DateTime>()
                .HaveColumnType("datetime");

            base.ConfigureConventions(configurationBuilder);
        }

    }
}
