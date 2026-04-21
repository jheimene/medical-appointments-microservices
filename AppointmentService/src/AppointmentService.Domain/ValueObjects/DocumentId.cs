
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentService.Domain.ValueObjects
{
    public record DocumentId
    {
        public string Number { get; init; } = default!;
        public IdentityDocumentType Type { get; init; }

        private DocumentId() { }

        public static DocumentId Create(string number, IdentityDocumentType type)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(number)) errors["number"] = ["El número de documento es requerido"];
                //throw new InvalidValueObjectException("document.number.required", "El número de documento es requerido");

            var normalizeNumber = Normalize(number);

            //Validate(normalizeNumber, type);
            if (!DocumentRules.IsValid(type, normalizeNumber)) errors["number"] = [$"Número de documento inválido para {type}. {DocumentRules.GetHint(type)}"];
            //throw new BusinessRuleViolationException("document.number.invalid", $"Número de documento inválido para {type}. {DocumentRules.GetHint(type)}");

            if (errors.Count > 0)
                throw new InvalidValueObjectException(
                    code: "document.invalid",   
                    message: "El documento no es válido",
                    errors: errors);

            return new DocumentId() { Number = normalizeNumber, Type = type };
        }

        private static string Normalize(string number) => number.Trim().Replace("-","").ToUpperInvariant();

        //private static void Validate(string number, IdentityDocumentType type) {
        //    switch (type)
        //    {
        //        case IdentityDocumentType.DNI:
        //            // Perú: 8 dígitos numéricos
        //            if (number.Length != 8 || !number.All(char.IsDigit))
        //                throw new BusinessRuleViolationException("document.number.invalid", "El DNI debe tener 8 dígitos numéricos.");
        //            break;

        //        case IdentityDocumentType.CE:
        //            // Perú: Generalmente 9 caracteres (pueden ser alfanuméricos según serie)
        //            if (number.Length < 8 || number.Length > 12)
        //                throw new BusinessRuleViolationException("document.number.invalid", "El Carnet de Extranjería no tiene una longitud válida.");
        //            break;

        //        case IdentityDocumentType.PASSPORT:
        //            // Estándar variable, pero suele estar entre 5 y 15 caracteres
        //            if (number.Length < 5 || number.Length > 20)
        //                throw new BusinessRuleViolationException("BusinessRuleViolationException", "El número de Pasaporte no es válido.");
        //            break;

        //        default:
        //            throw new NotSupportedException("Tipo de documento no soportado.");
        //    }
        //}

        public override string ToString() => $"{Type}: {Number}";

    }
}
