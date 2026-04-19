namespace ProductService.Domain.Common
{
    public static class Guard
    {
        public static void AgainstNullOrWhiteSpace(string? value, string code, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BusinessRuleViolationException(code, message);
        }

        public static void Against(bool condition, string code, string message)
        {
            if (condition) throw new BusinessRuleViolationException(code, message);
        }
    }
}
