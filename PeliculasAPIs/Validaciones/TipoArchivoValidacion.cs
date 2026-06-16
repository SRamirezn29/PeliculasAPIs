using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.Validaciones
{
    public class TipoArchivoValidacion : ValidationAttribute
    {
        private readonly string[] tiposValidados;
        public TipoArchivoValidacion(string[] tiposValidados)
        {
            this.tiposValidados = tiposValidados;
        }

        public TipoArchivoValidacion(GrupoTipoArchivo grupoTipoArchivo)
        {
            if(grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tiposValidados = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
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
            if (!tiposValidados.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo del archivo no es valido. Tipos permitidos: {string.Join(", ", tiposValidados)}");
            }
            return ValidationResult.Success;
        }
    }
}
