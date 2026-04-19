
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DispatchService.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder
                .UseSqlServer("Server=localhost;Database=bd_customer;User Id=admin;Password=abc123+.;TrustServerCertificate=True;")
                .EnableSensitiveDataLogging();

            return new ApplicationDbContext(optionsBuilder.Options, new NoOpPublisher());
        }
    }

    public sealed class NoOpPublisher : IPublisher
    {
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification => Task.CompletedTask;

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}


