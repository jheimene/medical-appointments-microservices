using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.Persistence.Interceptors;

namespace OrderService.Infrastructure.Persistence.Contexts
{
    public class OrderDbContext : DbContext
    {
        private readonly MetricsCommandInterceptor _metrics;

        public DbSet<Order> Orders { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options, MetricsCommandInterceptor metrics)
            : base(options)
        {
            _metrics = metrics;
        }

        //public static OrderDbContext Create(IMongoDatabase database) => 
        //    new(new DbContextOptionsBuilder<OrderDbContext>()
        //        .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
        //        .Options);
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
            modelBuilder.Ignore<DomainEvent>();
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Order>().ToCollection("Orders");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AuditableEntitySaveChangesInterceptor());
            optionsBuilder.AddInterceptors(_metrics);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
