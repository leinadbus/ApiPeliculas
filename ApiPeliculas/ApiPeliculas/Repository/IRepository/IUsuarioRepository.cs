using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<AppUsuario> GetUsuarios();

        AppUsuario GetUsuario(string usuarioId);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<UsuarioDatosDto> Registro(Usuarioregistrodto usuarioRegistroDto);
        //Task<UsuarioDatosDto> Login(Usuarioregistrodto usuarioRegistroDto);

    }
}
