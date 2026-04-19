using System.Security.Cryptography;
using System.Text;

namespace OrderService.Infrastructure.Idempotency.Helpers
{
    public static class RequestHashHelper
    {
        public static string ComputeSha256(string content)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
            return Convert.ToHexString(bytes);
        }
    }
}
