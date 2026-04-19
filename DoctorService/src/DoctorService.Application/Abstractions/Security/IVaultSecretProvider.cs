namespace ProductService.Application.Abstractions.Security
{
    public interface IVaultSecretProvider
    {
        Task<IDictionary<string, string>> GetSecretAsync(string secretPath);
        Task<string> GetSecretValueAsync(string secretPath, string secretKey);
    }
}
