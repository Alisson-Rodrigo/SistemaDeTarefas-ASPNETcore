using SistemaDeTarefas.Integracacao.Interfaces;
using SistemaDeTarefas.Integracacao.Refit;
using SistemaDeTarefas.Integracacao.Response;

namespace SistemaDeTarefas.Integracacao
{
    public class ViaCepIntegracao : IViaCepIntegracao
    {
        private readonly IViaCepIntegracaoRefit _viaCepIntegracaoRefit;

        public ViaCepIntegracao(IViaCepIntegracaoRefit viaCepIntegracaoRefit)
        {
            _viaCepIntegracaoRefit = viaCepIntegracaoRefit;
        }

        public async Task<ViaCepResponse> ObterDadosCep(string cep)
        {
            var response = await _viaCepIntegracaoRefit.ObterDadosCep(cep);
            if (response != null && response.IsSuccessStatusCode)
            {
                return response.Content;
            }
            return null;
        }
    }
}
