
namespace CustomerService.Domain.ValueObjects
{
    public sealed class AddressVo : ValueObject
    {
        public string Street { get; private set; } = default!;
        public string District { get; private set; } = default!;
        public string Province { get; private set; } = default!;
        public string Departament { get; private set; } = default!;
        public string? Reference { get; private set;  }
        //public string PostalCode { get; }

        private AddressVo() { }

        public static AddressVo Create (string street, string district, string province, string departament, string? reference = null) //, string country, string? state = null)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new BusinessRuleViolationException("La calle es requerida.", nameof(street));
            if (string.IsNullOrWhiteSpace(district)) throw new BusinessRuleViolationException("El distrito es requerido.", nameof(district));
            if (string.IsNullOrWhiteSpace(province)) throw new BusinessRuleViolationException("La provincia es requerida.", nameof(province));
            if (string.IsNullOrWhiteSpace(departament)) throw new BusinessRuleViolationException("El departamento es requerido.", nameof(departament));
            //if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required", nameof(country));

            var address = new AddressVo()
            {
                Street = street.Trim(),
                District = district.Trim(),
                Province = province.Trim(),
                Departament = departament.Trim(),
                Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim()
            };

            return address;
            //PostalCode = postalCode.Trim();
            //Country = country.Trim();
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Street;
            yield return District;
            yield return Province;
            yield return Departament;
            yield return Reference;
            //yield return PostalCode;
            //eld return Country;
        }

        public override string ToString()
        {
            var parts = new List<string> { Street };
            if (!string.IsNullOrEmpty(Reference)) parts.Add(Reference!);
            parts.Add($"{District}-{Province}-{Departament}");
            //parts.Add(PostalCode);
            //parts.Add(Country);
            return string.Join(", ", parts);
        }
    }
}
