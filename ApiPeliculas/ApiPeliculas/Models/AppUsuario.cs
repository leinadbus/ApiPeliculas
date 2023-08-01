using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Models
{
    public class AppUsuario : IdentityUser
    {
        //Añadir campos personalizados
        public string Nombre { get; set; }
    }
}
