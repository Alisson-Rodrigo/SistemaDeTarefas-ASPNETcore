using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Integracacao.Interfaces
{
    public interface IViaCepIntegracao
    {
        public Task<ViaCepResponse> ObterDadosCep(string cep);
    }
}
