using Refit;
using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Integracacao.Refit
{
    public interface IViaCepIntegracaoRefit
    {
        [Get("/ws/{cep}/json")]
        Task<ApiResponse<ViaCepResponse>> ObterDadosCep(string cep);
    }
}
