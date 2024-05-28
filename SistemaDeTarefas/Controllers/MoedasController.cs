using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Integracacao.Interfaces;
using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoedasController : ControllerBase
    {
        public readonly IDadosMoedasIntegracao _dadosMoedasIntegracao;

        public MoedasController(IDadosMoedasIntegracao dadosMoedasIntegracao)
        {
            _dadosMoedasIntegracao = dadosMoedasIntegracao;
        }

        [HttpGet("{moedas}")]
        public async Task<ActionResult<Moeda>> RetornaMoedas(string moedas)
        {
            try
            {
                var moedasJSON = await _dadosMoedasIntegracao.ObterDadosMoedas(moedas);
                if (moedasJSON == null)
                {
                    return BadRequest("Moedas não encontradas");
                }
                return Ok(moedasJSON);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
