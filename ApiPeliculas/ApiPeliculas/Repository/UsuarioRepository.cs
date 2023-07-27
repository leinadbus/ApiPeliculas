using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AplicationDbContext _bd;
        private string claveSecreta;
        public UsuarioRepository(AplicationDbContext _bd, IConfiguration config)
        {
            this._bd = _bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }
        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
           var usuarioBd = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario);
            if (usuarioBd == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Usuario> Registro(Usuarioregistrodto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            var usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role
            };

            _bd.Usuarios.Add(usuario);
            await _bd.SaveChangesAsync();
            usuario.Password = passwordEncriptado;
            return usuario;
        }

        public Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario = _bd.Usuarios.FirstOrDefault(
                u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                && u.Password == passwordEncriptado
                );

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null)
            {
                var usuarioRespuesta = new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
                return usuarioRespuesta;
            }

            //Aqui existe el usuario
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            var usuarioLoginRespuestaDtp = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
            return usuarioLoginRespuestaDtp;
        }


        public static string obtenermd5 (string valor )
        {
            MD5CryptoServiceProvider x = new();
            byte[] data = System.Text.Encoding.UTF8.GetBytes( valor );
            data = x.ComputeHash( data );
            string resp = "";
            for (int i =0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
    }
}
