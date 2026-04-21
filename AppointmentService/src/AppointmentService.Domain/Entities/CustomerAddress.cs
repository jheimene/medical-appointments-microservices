
namespace AppointmentService.Domain.Entities
{
    public sealed class CustomerAddress : AuditableEntity<CustomerAddressId, string>
    {               
        public CustomerId CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        // El "Alias" de la dirección (Ej. "Mi Casa", "Mi Trabajo")
        public string Label { get; private set; } = default!;

        public AddressVo Address { get; private set; } = null!;
        public bool IsDefault { get; private set; }

        private CustomerAddress() { }

        public static CustomerAddress Create(CustomerId customerId, string label, AddressVo address, string createdBy, bool isDefault = false)
        {
            if (string.IsNullOrWhiteSpace(label)) throw new ArgumentNullException("La etiqueta de dirección es requerida.", nameof(label));

            var customerAddress = new CustomerAddress()
            {
                Id = CustomerAddressId.NewId(),
                CustomerId = customerId,
                Label = label,
                Address = address,
                IsDefault = isDefault
            };

            customerAddress.SetCreated(createdBy);
           
            return customerAddress;
        }

        public void MarkAsDefault() => IsDefault = true;
        public void UnMarkAsDefault() => IsDefault = false;
    }
}
