using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefasRepositorio
    {
        private readonly SistemaDeTarefasDbContext _context;

        public TarefaRepositorio(SistemaDeTarefasDbContext context)
        {
            _context = context;
        }

        public async Task<TarefaModel> BuscarTarefaPorId(int id)
        {
            return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
             
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _context.Tarefas.ToListAsync();
        }
        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            if (tarefa != null) { 
                await _context.Tarefas.AddAsync(tarefa);
                await _context.SaveChangesAsync();
                return tarefa;
            }
            return null;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            var tarefaPorId = await BuscarTarefaPorId(id);
            tarefaPorId.Titulo = tarefa.Titulo;
            tarefaPorId.Descricao = tarefa.Descricao;
            tarefaPorId.Status = tarefa.Status;
            _context.Update(tarefaPorId);
            await _context.SaveChangesAsync();
            return tarefaPorId;
        }

        public async Task<bool> Deletar(int id)
        {
            var tarefaDeleter = await BuscarTarefaPorId(id);
            _context.Tarefas.Remove(tarefaDeleter);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
