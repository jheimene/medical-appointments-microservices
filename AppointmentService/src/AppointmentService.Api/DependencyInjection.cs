
namespace OrderService.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Orders API",
                    Version = "v1",
                    Description = "Microservicio de Ordenes",
                });
            });

            services.AddHealthChecks();

            return services;
        }
    }
}
