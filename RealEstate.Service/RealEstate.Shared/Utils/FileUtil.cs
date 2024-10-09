using System.Drawing;
using System.Drawing.Imaging;

namespace RealEstate.Shared.Utils
{
    public static class FileUtil
    {
        private static string _localBasePath = "Files";
        private static string _libraryBasePath = Directory.GetCurrentDirectory();

        public static string SaveImage(string module, string folderName, string fileBase64)
        {
            string fileUrl = string.Empty;

            // Crea la ruta para la nueva carpeta
            var folderPath = Path.Combine(_libraryBasePath, _localBasePath, module, folderName);

            // Crea la carpeta si no existe
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Procesa las imágenes
            if (IsValidImageBase64(fileBase64, out string fileExtension))
            {
                fileUrl = SaveLocalImage(fileBase64, folderName, folderPath, fileExtension);
            }
            else
            {
                throw new InvalidDataException("El archivo base64 no representa una imagen válida.");
            }

            return fileUrl;
        }

        private static string SaveLocalImage(string fileBase64, string codeInternal, string folderPath, string fileExtension)
        {
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            // Convierte el string base64 a un array de bytes
            var imageBytes = Convert.FromBase64String(fileBase64);

            // Guarda la imagen en el archivo
            File.WriteAllBytes(filePath, imageBytes);

            return Path.Combine("Files", "PropertyImages", codeInternal, fileName);
        }

        public static string GetFullPath(string relativePath)
        {
            // Obtiene la ruta base desde la configuración de la aplicación
            var basePath = Directory.GetCurrentDirectory();
            return Path.Combine(_localBasePath, relativePath).Replace("\\", "/");
        }

        private static bool IsValidImageBase64(string base64String, out string fileExtension)
        {
            fileExtension = "jpg"; // Valor predeterminado

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
