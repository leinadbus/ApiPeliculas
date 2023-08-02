using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepository(AplicationDbContext _bd, IConfiguration config, 
            RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager,
            IMapper mapper)
        {
            this._bd = _bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuarios.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
           var usuarioBd = _bd.AppUsuarios.FirstOrDefault(u => u.UserName == usuario);
            if (usuarioBd == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UsuarioDatosDto> Registro(Usuarioregistrodto usuarioRegistroDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            var usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                //Password = passwordEncriptado,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                //Role = usuarioRegistroDto.Role
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);

            if(result.Succeeded)
            {
                //Solo la primera vez para crear los roles
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = _bd.AppUsuarios.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);

                //opcion 1
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre
                //};

                //opcion 2
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);

            }
            //_bd.Usuarios.Add(usuario);
            //await _bd.SaveChangesAsync();
            //usuario.Password = passwordEncriptado;
            //return usuario;
            return new UsuarioDatosDto();
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario = _bd.AppUsuarios.FirstOrDefault(
                u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
                //&& u.Password == passwordEncriptado
                );

            bool isValida = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null || isValida == false)
            {
                var usuarioRespuesta = new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
                return usuarioRespuesta;
            }

            //Aqui existe el usuario
            var roles = await _userManager.GetRolesAsync(usuario);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            var usuarioLoginRespuestaDtp = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)
            };
            return usuarioLoginRespuestaDtp;
        }


        //public static string obtenermd5 (string valor )
        //{
        //    MD5CryptoServiceProvider x = new();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes( valor );
        //    data = x.ComputeHash( data );
        //    string resp = "";
        //    for (int i =0; i < data.Length; i++)
        //    {
        //        resp += data[i].ToString("x2").ToLower();
        //    }
        //    return resp;
        //}
    }
}
