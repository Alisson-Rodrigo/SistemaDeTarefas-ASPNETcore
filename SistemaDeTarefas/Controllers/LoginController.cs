using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Repositorios.Interfaces;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SistemaDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IConfiguration _configuration;


        public LoginController(IUsuarioRepositorio usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepositorio = usuarioRepository;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorLogin(login.Login);
            if (usuario != null) {
                if (usuario.VerificarSenha(login.Senha)){
                    var token = gerarTokenJWT(usuario.Nome);
                    return Ok(new {token});
                }
                else
                {
                    return BadRequest("Usuário ou senha incorretos");
                }
            }
            else
            {
                return BadRequest("Usuário ou senha incorretos");
            }

        }

        private string gerarTokenJWT(string nome)
        {
            string key_secret = _configuration["JWT_SECRET"];
            var key = Encoding.UTF8.GetBytes(key_secret);
            var credencial = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("Login", nome),
                new Claim(ClaimTypes.Name, "Usuario"),
            };

            var token = new JwtSecurityToken(
                    issuer: "SistemaDeTarefas",
                    audience: "SistemaDeTarefas",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credencial
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost("adicionar")]
        public async Task<ActionResult<UsuarioModel>> AdicionarUsuario([FromBody] UsuarioModel usuario)
        {
            try
            {
                UsuarioModel usuarioAdicionado = await _usuarioRepositorio.Adicionar(usuario);
                return Ok(usuarioAdicionado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
