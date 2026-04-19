namespace CustomerService.Domain.Helpers
{
    public static class DomainErrors
    {
        public static Dictionary<string, string[]> Prefix(string prefix, IReadOnlyDictionary<string, string[]> errors) => errors.ToDictionary(k => $"{prefix}.{k.Key}", v => v.Value);
    }
}
