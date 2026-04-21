using AppointmentService.Application.Abstractions.Clients;
using AppointmentService.Application.Abstractions.Secrets;
using AppointmentService.Application.Commmon.Interfaces;
using AppointmentService.Domain.Interfaces;
using AppointmentService.Infrastructure.Caching;
using AppointmentService.Infrastructure.Clients;
using AppointmentService.Infrastructure.Persistence.Contexts;
using AppointmentService.Infrastructure.Persistence.Repositories;
using AppointmentService.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace AppointmentService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISecretProvider>(sp =>
                new LocalSecretProvider(configuration));
            services.AddSingleton<InMemorySecretCache>();
            services.AddPersistence(configuration);
            services.AddResiliencePolicies(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            return services;
        }

        public static void AddResiliencePolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        Console.WriteLine($"Reintento {retryAttempt} despues de {timespan.TotalSeconds} segundos");
                    });

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                    onBreak: (result, timespan) =>
                        Console.WriteLine($"Circuit breaker abierto por {timespan.TotalSeconds} segundos"),
                    onReset: () =>
                        Console.WriteLine("Circuit breaker cerrado"));

            var patientServiceUrl = configuration.GetSection("Services:PatientService").Value ?? "https://localhost:7000";
            var doctorServiceUrl = configuration.GetSection("Services:DoctorService").Value ?? "https://localhost:7001";

            services.AddHttpClient<IPatientServiceClient, PatientServiceClient>(client =>
            {
                client.BaseAddress = new Uri(patientServiceUrl);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            services.AddHttpClient<IDoctorServiceClient, DoctorServiceClient>(client =>
            {
                client.BaseAddress = new Uri(doctorServiceUrl);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);
        }
    }
}