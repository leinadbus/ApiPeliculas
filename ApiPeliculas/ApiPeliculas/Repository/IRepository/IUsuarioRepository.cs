using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(Usuarioregistrodto usuarioRegistroDto);

    }
}
