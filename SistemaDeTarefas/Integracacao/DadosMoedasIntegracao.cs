using SistemaDeTarefas.Integracacao.Interfaces;
using SistemaDeTarefas.Integracacao.Response;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SistemaDeTarefas.Integracacao.Refit;
using Refit;

namespace SistemaDeTarefas.Integracacao
{
    public class DadosMoedasIntegracao : IDadosMoedasIntegracao
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DadosMoedasIntegracao> _logger;


        public DadosMoedasIntegracao(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //usando o httpClient para fazer a requisição
        public async Task<DadosMoedasResponse> ObterDadosMoedas(string moedas)
        {
            var response = await _httpClient.GetAsync($"/json/all/{moedas}");
            if (response.IsSuccessStatusCode)
            {
                var dados = await response.Content.ReadAsStringAsync();
                var moedasDict = JsonConvert.DeserializeObject<Dictionary<string, Moeda>>(dados);

                // Encapsulando no DadosMoedasResponse
                var result = new DadosMoedasResponse { Moedas = moedasDict };
                return result;
            }
            return null;
        }

    }
}
