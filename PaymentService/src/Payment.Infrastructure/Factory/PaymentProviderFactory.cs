

using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Providers;

namespace PaymentService.Infrastructure.Factory
{
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProvider GetProvider(string method)
        {

            return method.ToLower() switch
            {
                "safetypay" => _serviceProvider.GetRequiredService<MockSafetypayProvider>(),
                "paypal" => _serviceProvider.GetRequiredService<MockPaypalProvider>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
