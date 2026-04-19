
namespace PaymentService.Application.Interfaces
{
    public interface IPaymentProvider
    {
        Task<string> ProccessPaymentAsync(decimal amout, string currency, string user);
    }
}
