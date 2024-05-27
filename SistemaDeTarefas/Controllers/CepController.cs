using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Integracacao;
using SistemaDeTarefas.Integracacao.Interfaces;
using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CepController : ControllerBase
    {

        private readonly IViaCepIntegracao _viaCepIntegracao;
        private readonly IDadosMoedasIntegracao _dadosMoedasIntegracao;
        public CepController(IViaCepIntegracao viaCepIntegracao, IDadosMoedasIntegracao dadosMoedasIntegracao)
        {
            _viaCepIntegracao = viaCepIntegracao;
            _dadosMoedasIntegracao = dadosMoedasIntegracao;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<ViaCepResponse>> ListarDadosDoEndereco(string cep)
        {
            try
            {
                var endereco = await _viaCepIntegracao.ObterDadosCep(cep);
                if (endereco == null)
                {
                    return BadRequest("CEP não encontrado");
                }
                return Ok(endereco);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
