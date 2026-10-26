using Banco.API.Domain.Entities;
using Banco.API.Domain.Errors;
using Banco.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Banco.API.Infrastructure.Service
{
    public class ClienteService : IClienteService
    {
        private readonly ILogger<ClienteService> _logger;
        private readonly RestClient _restClient;

        public ClienteService(ILogger<ClienteService> logger)
        {
            _logger = logger;
            _restClient = new RestClient("https://viacep.com.br/ws/");
        }

        public async Task<EnderecoModel?> ObterEnderecoPorCepAsync(string cep)
        {
            var request = new RestRequest($"{cep}/json/", Method.Get);
            var response = await _restClient.ExecuteAsync<EnderecoModel>(request);

            if (!response.IsSuccessful || response.Data == null)
                throw new NoContentException($"Não foi localizado o endereco para esta cep: {cep}");

            return response.Data;
        }
    }
}
