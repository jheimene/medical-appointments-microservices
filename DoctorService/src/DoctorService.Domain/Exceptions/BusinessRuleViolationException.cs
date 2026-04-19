namespace ProductService.Domain.Exceptions
{
    internal sealed class BusinessRuleViolationException : DomainException
    {
        public BusinessRuleViolationException(string code, string message): base(code, message) { } 
    }
}
