
namespace OrderService.Infrastructure.Configuration.Secrets
{
    public sealed class AzureKeyVaultOptions
    {
        public const string SectionName = "AzureKeyVault";
        public string SecretName { get; set; } = string.Empty;
    }
}
