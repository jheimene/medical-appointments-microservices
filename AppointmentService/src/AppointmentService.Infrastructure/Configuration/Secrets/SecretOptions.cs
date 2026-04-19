namespace OrderService.Infrastructure.Configuration.Secrets
{
    public sealed class SecretOptions
    {
        public const string SectionName = "Secrets";
        public SecretProviderType ProviderName { get; set; } = SecretProviderType.HashiCorpVault;
        public HashiCorpVaultOptions Vault { get; set; } = new();
        public AwsSecretsManagerOptions SecretsManager { get; set; } = new();
    }

    public enum SecretProviderType
    {
        AzureKeyVault,
        AWSSecretsManager,
        HashiCorpVault,
        LocalFile,
        EnvironmentVariables
    }
}
