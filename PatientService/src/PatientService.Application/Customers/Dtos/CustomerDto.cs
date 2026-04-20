namespace PatientService.Application.Customers.Dtos
{
    public sealed record CustomerDto
    {
        public Guid CustomerId { get; init; }
        public string Name { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string DocumentType { get; init; } = default!;
        public string DocumentNumber { get; init; } = default!;
        public string? Email { get; init; }
        public bool IsActive { get; set; }

    }
}
