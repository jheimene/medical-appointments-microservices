

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetProvider(string method);
    }
}
