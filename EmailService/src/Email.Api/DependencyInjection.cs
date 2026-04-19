using Email.Api.Models;
using Email.Api.Services;

namespace Email.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmailApi(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar opciones de SMTP
            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));

            // Registrar el servicio de envío de correos
            services.AddTransient<IEmailSender, SmtpEmailSender>();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            return services;
        }

    }
}
