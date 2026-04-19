namespace OrderService.Infrastructure.Configuration.Secrets
{
    public sealed class AwsSecretsManagerOptions
    {
        public const string SectionName = "AwsSecretsManager";
        public string SecretName { get; set; } = string.Empty;
    }
}
