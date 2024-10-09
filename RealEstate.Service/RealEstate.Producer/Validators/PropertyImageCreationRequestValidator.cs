namespace RealEstate.Producer.Validators
{
    using FluentValidation;
    using RealEstate.Producer.Requests;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Text.RegularExpressions;

    public class PropertyImageCreationRequestValidator : AbstractValidator<PropertyImageCreationRequest>
    {
        public PropertyImageCreationRequestValidator()
        {
            RuleFor(x => x.FileBase64)
                .NotEmpty().WithMessage("El imagen de la propiedad en formato Base64 es obligatorio.")
                .Must(BeValidBase64).WithMessage("La imagen de la propiedad es un archivo que debe estar en un formato Base64 válido.");

            RuleFor(x => x.Enable)
                .NotNull().WithMessage("El estado de habilitación es obligatorio.");
        }

        private bool BeValidBase64(string base64String)
        {
              string fileExtension = "jpg"; // Valor predeterminado

            try
            {
                var imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                using var image = Image.FromStream(ms);
                fileExtension = image.RawFormat.Equals(ImageFormat.Jpeg) ? "jpg" :
                                image.RawFormat.Equals(ImageFormat.Png) ? "png" :
                                image.RawFormat.Equals(ImageFormat.Gif) ? "gif" :
                                image.RawFormat.Equals(ImageFormat.Bmp) ? "bmp" :
                                null;

                if (fileExtension == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
