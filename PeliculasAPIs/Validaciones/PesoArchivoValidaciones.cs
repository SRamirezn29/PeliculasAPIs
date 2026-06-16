using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.Validaciones
{
    public class PesoArchivoValidaciones : ValidationAttribute
    {
        private readonly int pesoMaximoEnMegabytes; 
        public PesoArchivoValidaciones(int PesoMaximoEnMegabytes)
        {
            pesoMaximoEnMegabytes = PesoMaximoEnMegabytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)  
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;

            }

            if (formFile.Length > pesoMaximoEnMegabytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso de la imagen no puede ser mayor a {pesoMaximoEnMegabytes} MB");
            }

            return ValidationResult.Success;
        }
    }
}
