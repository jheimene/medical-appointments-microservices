namespace OrderService.Infrastructure.Configuration.Secrets
{
    public sealed class HashiCorpVaultOptions
    {
        public const string SectionName = "Vault";

        public string Address { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string SecretId { get; set; } = string.Empty;
        public string AppRoleMountPoint { get; set; } = "approle";
        public string KvMountPoint { get; set; } = "secrets";
        public string SecretPath { get; set; } = string.Empty;

        public int CacheTtlSeconds { get; set; } = 300;
        public bool ReloadOnEachRead { get; set; } = false;
    }
}
