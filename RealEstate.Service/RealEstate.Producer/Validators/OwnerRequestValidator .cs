using FluentValidation;
using RealEstate.Producer.Requests;

namespace RealEstate.Producer.Validators
{
    public class OwnerRequestValidator:  AbstractValidator<OwnerRequest>
    {
        public OwnerRequestValidator()
        {
            // Validación para Name: obligatorio, longitud mínima de 2 caracteres y máxima de 100
            RuleFor(owner => owner.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres.");

            // Validación para Address: obligatorio y longitud mínima de 10 caracteres
            RuleFor(owner => owner.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MinimumLength(10).WithMessage("La dirección debe tener al menos 10 caracteres.");

            // Validación para Photo: campo obligatorio y debe ser una URL válida
            RuleFor(owner => owner.Photo)
                .NotEmpty().WithMessage("La foto del propietario es obligatoria.")
                .Must(BeAValidUrl).WithMessage("La foto del propietario debe ser una URL válida.");

            // Validación para Birthday: no debe ser una fecha futura
            RuleFor(owner => owner.Birthday)
                .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento no puede ser en el futuro.");
        }

        // Método personalizado para validar que la cadena sea una URL válida
        private bool BeAValidUrl(string photoUrl)
        {
            return Uri.TryCreate(photoUrl, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
