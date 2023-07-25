using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CrearCategoriaDto
    {
        //Esta validación es requerida para el nombre de la categoría
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage ="El máximo de caracterer es de 100!")]
        public string Nombre { get; set; }
    }
}
