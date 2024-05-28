using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Integracacao.Interfaces
{
    public interface IViaCepIntegracao
    {
        Task<ViaCepResponse> ObterDadosCep(string cep);
    }
}
