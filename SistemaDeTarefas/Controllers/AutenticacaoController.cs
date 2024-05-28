using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Repositorios.Interfaces;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SistemaDeTarefas.Helper.Interfaces;

namespace SistemaDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IConfiguration _configuration;
        private readonly IEnviarEmailRecuperacao _enviar;


        public AutenticacaoController(IUsuarioRepositorio usuarioRepository, IConfiguration configuration, IEnviarEmailRecuperacao enviar)
        {
            _usuarioRepositorio = usuarioRepository;
            _configuration = configuration;
            _enviar = enviar;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorLogin(login.Login);
            if (usuario != null) {
                if (usuario.VerificarSenha(login.Senha)){
                    var token = gerarTokenJWT(usuario);
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

        private string gerarTokenJWT(UsuarioModel usuario)
        {
            string key_secret = _configuration["JWT_SECRET"];
            var key = Encoding.UTF8.GetBytes(key_secret);
            var credencial = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserId", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),

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

        [HttpPost("atualizar_senha")]
        public async Task<ActionResult> AtualizarSenha([FromBody] EmailModel email)
        {
            try
            {
                UsuarioModel usuarioAtualizado = await _usuarioRepositorio.BuscarUsuarioPorEmail(email.Email);
                if (usuarioAtualizado != null)
                {
                    string novaSenha = usuarioAtualizado.GerarNovaSenha();
                    string mensagem = $"Olá, {usuarioAtualizado.Nome} sua nova senha é: {novaSenha}";
                    bool emailEnviado = await _enviar.SendEmail(email.Email, "Redefinição de senha", mensagem);
                    if (emailEnviado)
                    {
                        await _usuarioRepositorio.Atualizar(usuarioAtualizado, usuarioAtualizado.Id);
                        return Ok("Email enviado com sucesso");
                    }
                    else
                    {
                        return BadRequest("Erro ao enviar e-mail");
                    }
                }
                else
                {
                    return BadRequest("Usuário não encontrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
