
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PatientService.Infrastructure.Persistence.Configurations
{
    public class CustomerIdConversion : ValueConverter<CustomerId, Guid>
    {
        public CustomerIdConversion() : base(
            id => id.Value, // Convertir CustomerId a Guid para almacenar en la base de datos
            value => new CustomerId(value)) // Convertir Guid de la base de datos a CustomerId al leer
        {
        }
    }

    public class CustomerAddressIdConversion : ValueConverter<CustomerAddressId, Guid>
    {
        public CustomerAddressIdConversion() : base(
            id => id.Value, // Convertir CustomerId a Guid para almacenar en la base de datos
            value => new CustomerAddressId(value)) // Convertir Guid de la base de datos a CustomerId al leer
        {
        }
    }
}
