namespace RealEstate.Producer.Validators
{
    using FluentValidation;
    using RealEstate.Producer.Requests;

    public class PropertyTraceCreationRequestValidator : AbstractValidator<PropertyTraceCreationRequest>
    {
        public PropertyTraceCreationRequestValidator()
        {
            RuleFor(x => x.DateSale)
                .NotEmpty().WithMessage("La fecha de la venta es obligatoria.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de la venta no puede ser futura.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del individuo o entidad es obligatorio.")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("El valor de la propiedad debe ser mayor a 0.");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0).WithMessage("El impuesto debe ser igual o mayor a 0.");
        }
    }
}
