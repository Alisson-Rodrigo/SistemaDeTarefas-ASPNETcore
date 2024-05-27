using Refit;
using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Integracacao.Interfaces
{
    public interface IDadosMoedasIntegracao
    {
        Task<DadosMoedasResponse> ObterDadosMoedas(string moedas);
    }
}
