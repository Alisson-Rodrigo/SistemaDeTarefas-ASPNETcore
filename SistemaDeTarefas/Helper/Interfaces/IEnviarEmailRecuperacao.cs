using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Helper.Interfaces
{
    public interface IEnviarEmailRecuperacao
    {
        Task<bool> SendEmail(string email, string assunto, string mensagem);

    }
}
