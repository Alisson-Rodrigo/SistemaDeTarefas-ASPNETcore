using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefasRepositorio _tarefasRepositorio;
        public TarefasController(ITarefasRepositorio tarefasRepositorio)
        {
            _tarefasRepositorio = tarefasRepositorio;
        }


        [HttpGet]
        public async Task<ActionResult<List<TarefaModel>>> BuscarTodasTarefas()
        {
            List<TarefaModel> tarefas = await _tarefasRepositorio.BuscarTodasTarefas();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<TarefaModel>>> BuscarTarefaPorId(int id)
        {
            var tarefa = await _tarefasRepositorio.BuscarTarefaPorId(id);
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<TarefaModel>> AdicionarTarefa([FromBody] TarefaModel tarefa)
        {
            TarefaModel tarefaAdicionada = await _tarefasRepositorio.Adicionar(tarefa);
            return Ok(tarefaAdicionada);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarefaModel>> AtualizarTarefa(int id, [FromBody] TarefaModel tarefa)
        {
            TarefaModel tarefaAtualizada = await _tarefasRepositorio.Atualizar(tarefa, id);
            return Ok(tarefaAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletarTarefa(int id)
        {
            bool tarefaDeletada = await _tarefasRepositorio.Deletar(id);
            return Ok(tarefaDeletada);
        }
    }
}
