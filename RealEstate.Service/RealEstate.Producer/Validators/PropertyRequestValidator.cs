using FluentValidation;
using RealEstate.Producer.Requests;

namespace RealEstate.Producer.Validators
{
    public class PropertyCreationRequestValidator : PropertyBaseValidator<PropertyCreationRequest>
    {
        public PropertyCreationRequestValidator()
        {
            RuleFor(x => x.Traces)
                .NotNull().WithMessage("La colección de trazas no puede ser nula.")
                .ForEach(trace => trace.SetValidator(new PropertyTraceCreationRequestValidator()));

            RuleFor(x => x.Images)
                .NotNull().WithMessage("La colección de imágenes no puede ser nula.")
                .ForEach(image => image.SetValidator(new PropertyImageCreationRequestValidator()));
        }
    }

    public class PropertyUpdateRequestValidator : PropertyBaseValidator<PropertyUpdateRequest>
    {
        public PropertyUpdateRequestValidator()
        {
            RuleFor(x => x.IdProperty)
                .GreaterThan(0).WithMessage("El identificador de la propiedad debe ser mayor a 0.");
        }
    }
}
