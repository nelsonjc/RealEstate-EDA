using FluentValidation;
using RealEstate.Producer.Requests;

namespace RealEstate.Producer.Validators
{
    public class PropertyBaseValidator<T> : AbstractValidator<T> where T : PropertyBase
    {
        public PropertyBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la propiedad es obligatorio.")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección de la propiedad es obligatoria.")
                .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(x => x.CodeInternal)
                .NotEmpty().WithMessage("El código interno es obligatorio.")
                .Length(1, 50).WithMessage("El código interno debe tener entre 1 y 50 caracteres.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1800, DateTime.Now.Year).WithMessage($"El año debe estar entre 1800 y {DateTime.Now.Year}.");

            RuleFor(x => x.Owner)
                .NotNull().WithMessage("El propietario es obligatorio.");
        }
    }
}
