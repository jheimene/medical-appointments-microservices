
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Repositories;

namespace PaymentService.Infrastructure.Persistence
{
    public static class ConnectionFactory
    {

        public static void AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddSingleton<IPaymentRepository>(new PaymentRepositoryDapper(connectionString));
        }
    }
}
