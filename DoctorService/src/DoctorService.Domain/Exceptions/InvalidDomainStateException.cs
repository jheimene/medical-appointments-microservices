namespace ProductService.Domain.Exceptions
{
    internal sealed class InvalidDomainStateException : DomainException
    {
        public InvalidDomainStateException(string code, string message) : base(code, message)
        {
        }
    }
}
