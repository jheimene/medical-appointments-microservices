
using AppointmentService.Domain.Enums;
using AppointmentService.Domain.Helpers;
using System.Xml.Linq;

namespace AppointmentService.Domain.Entities
{
    public sealed class Customer : AggregateRoot<CustomerId, string>
    {
        // Core data
        public string Name { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string FullName => $"{Name} {LastName}".Trim();
        public DocumentId Document { get; private set; } = default!;

        // Contact
        public Email? Email { get; private set; }
        public PhoneNumber? Phone { get; private set; }

        public bool IsMailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }

        private readonly List<CustomerAddress> _address = new();
        public IReadOnlyCollection<CustomerAddress> Address => _address.AsReadOnly();

        // Personal data
        public DateOnly? BirthDate { get; private set; }
        public Gender? Gender { get; private set; }

        // Status
        public CustomerStatus Status { get; set; }

        private Customer() { } // Para ORM con EF Core

        public static Customer Create(
            string name,
            string lastName,
            IdentityDocumentType docType,
            string docNumber,
            string createdBy,
            DateTime createdAt,
            string? email = null,
            string? phone = null,
            DateOnly? birthDate = null,
            Gender? gender = null
        )
        {
            // Validaciones propias del dominio
            //if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es requerido", nameof(name));
            //if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("El apellido es requerido", nameof(lastName));

            // Validar campos obligatorios
            var errors_fields = ValidateFieldsRequired(name, lastName, docNumber);
            if (errors_fields.Count > 0)
            {
                throw new DomainValidationException(
                    code: "cutomer.required_fields",
                    message: "Reglas de negocio no cumplidas.",
                    errors: errors_fields);
            }

            var errors = new Dictionary<string, string[]>();
            var customer = new Customer();
            try
            {

                var document = DocumentId.Create(docNumber, docType);
                var emailVo = (email is not null) ? Email.Create(email ?? "") : null;
                var phoneVo = (phone is not null) ? new PhoneNumber(phone ?? "") : null;

                ValidateBirthDate(birthDate);

                customer = new Customer
                {
                    Id = CustomerId.NewId(),
                    Name = name,
                    LastName = lastName,
                    Document = document,
                    Email = emailVo,
                    Phone = phoneVo,
                    Status = CustomerStatus.Active
                };

                customer.SetCreated("system");
            }
            catch (InvalidValueObjectException iv)
            {
                var prefix = iv.Code switch
                {
                    "document.invalid" => "documentNumber",
                    _ => ""
                };

                var errorsVo = iv.Errors.ToDictionary(k => $"{prefix}", v => v.Value);

                //foreach (var kv in DomainErrors.Prefix($"{prefix}", iv.Errors))
                foreach (var kv in errorsVo)
                        errors[kv.Key] = kv.Value;
                
            }

            if (errors.Count > 0)
                throw new DomainValidationException("customer.invalid", "Domain validation failed", errors);

            return customer;
        }

        private static Dictionary<string, string[]> ValidateFieldsRequired(string name, string lastName, string documentNumber)
        {
            var errors = new Dictionary<string, string[]>();

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(name))
                errors["name"] = new[] { "El nombre es requerido." };

            if (string.IsNullOrWhiteSpace(lastName))
                errors["lastName"] = new[] { "El apellido es requerido." };

            if (string.IsNullOrWhiteSpace(documentNumber))
                errors["documentNumber"] = new[] { "El número de documento es requerido." };

            return errors;
        }


        public void ChangeName(string name, string modifiedBy)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(name)) throw new BusinessRuleViolationException("customer.name.required", "El nombre es obligatorio");
            Name = name.Trim();
            SetModified(modifiedBy);
        }

        public void ChangeLastName(string lastName, string modifiedBy)
        {
            EnsureActive();
            if (string.IsNullOrWhiteSpace(lastName)) throw new BusinessRuleViolationException("customer.lastName.required", "El nombre es obligatorio");
            LastName = lastName.Trim();
            SetModified(modifiedBy);
        }

        public void ChangeDocument(IdentityDocumentType type, string number, string modifiedBy)
        {
            EnsureActive();
            Document = DocumentId.Create(number, type);
            SetModified(modifiedBy);
        }
        public void ChangeEmail(string email, string modifiedBy)
        {
            EnsureActive();
            var emailNew = Email.Create(email);
            if (emailNew.Equals(Email)) return; // throw new BusinessRuleViolationException("customer.email", "No se ha proporcionado un nuevo email.");
            Email = emailNew;
            IsMailVerified = false;
            SetModified(modifiedBy);
        }

        public void VerifyEmail(string modifiedBy)
        {
            EnsureActive();
            if (Email is null) throw new BusinessRuleViolationException("customer.email.missing", "No se puede verificar un email vacío.");
            IsMailVerified = true;
            SetModified(modifiedBy);
        }

        public void ChangePhone(string phone, string modifiedBy)
        {
            EnsureActive();
            Phone = new PhoneNumber(phone);
            SetModified(modifiedBy);
        }

        public void VerifiedPhone(string modifiedBy)
        {
            EnsureActive();
            if (Phone is null) throw new BusinessRuleViolationException("customer.phone.missing", "No se puede verificar un teléfono vacío.");
            IsPhoneVerified = true;
            SetModified(modifiedBy);
        }

        public void Activate(string modifiedBy)
        {
            if (Status == CustomerStatus.Active) return;
            Status = CustomerStatus.Active;
            SetModified(modifiedBy);
        }

        public void Deactivate(string modifiedBy)
        {
            if (Status != CustomerStatus.Active)
                throw new InvalidDomainStateException("customer.status.invalid", "Solo un cliente activo puede desactivarse.");
            Status = CustomerStatus.Inactive;
            SetModified(modifiedBy);
        }

        private void EnsureActive()
        {
            if (Status != CustomerStatus.Active)
                throw new InvalidDomainStateException("customer.inactive", "El cliente no está activo para esta operación.");
        }

        public CustomerAddress AddAddress(
            CustomerId customerId,
            string label,
            string street,
            string district,
            string province,
            string departament,
            string modifiedBy,
            string? reference,
            bool isDefault = false
        )
        {
            EnsureActive();

            var addressVo = AddressVo.Create(street, district, province, departament, reference);
            var newAddress = CustomerAddress.Create(Id, label, addressVo, modifiedBy, isDefault);

            // Si ya existe una dirección por defecto la cambiamos
            var currentAddressDefault = _address.FirstOrDefault(a => a.IsDefault);
            if (currentAddressDefault is not null && newAddress.IsDefault)
                currentAddressDefault.UnMarkAsDefault();
            //throw new BusinessRuleViolationException("customer.address.is_default_min");

            _address.Add(newAddress);

            // Asignar al menos 1 dirección por defecto
            if (_address.Count == 1 || !newAddress.IsDefault)
                SetDefaultAddress(newAddress.Id);

            SetModified(modifiedBy);
            return newAddress;
        }

        public void RemoveAddress(
            CustomerAddressId addressId,
            string modifiedBy
        )
        {
            EnsureActive();

            var address = _address.FirstOrDefault(a => a.Id == addressId);
            if (address is null)
                throw new NotFoundDomainException("customer.address.not_found", $"No existe la dirección '{addressId}'.");

            _address.Remove(address);

            // Reasigna default si quitamos el default
            if (address.IsDefault && _address.Count > 0)
            {
                var firstAddress = _address.FirstOrDefault();
                SetDefaultAddress(firstAddress!.Id);
            }

            SetModified(modifiedBy);
        }

        public void SetDefaultAddress(CustomerAddressId addressId)
        {
            EnsureActive();
            var address = _address.FirstOrDefault(a => a.Id == addressId);
            if (address is null)
                throw new NotFoundDomainException("customer.address.not_found", $"No existe la dirección '{addressId}'.");

            address.MarkAsDefault();
        }

        public void ChangeBirthDate(DateOnly? birthDate, string modifiedBy)
        {
            EnsureActive();
            ValidateBirthDate(birthDate);
            BirthDate = birthDate;
            SetModified(modifiedBy);
        }

        private static void ValidateBirthDate(DateOnly? birthDate)
        {
            if (birthDate is null) return;

            var today = DateOnly.FromDateTime(DateTime.Now);
            // No puede ser fecha futura
            if (birthDate.Value > today)
                throw new BusinessRuleViolationException("customer.birthdate.future", "La fecha de nacimiento no puede ser futura.");

            // Debe ser mayor de edad
            int age = today.Year - birthDate.Value.Year;
            if (birthDate > today.AddYears(-age)) age--;
            if (age < 18)
                throw new BusinessRuleViolationException("customer.birthdate.adult", "Debe ser mayor de edad.");
        }

    }
}
