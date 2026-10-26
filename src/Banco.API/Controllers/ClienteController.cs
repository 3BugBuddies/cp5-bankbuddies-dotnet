using Banco.API.Application.Dtos;
using Banco.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace Banco.API.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteUseCase _clienteUseCase;

        public ClienteController(IClienteUseCase clienteUseCase)
        {
            _clienteUseCase = clienteUseCase;
        }

        [HttpGet]
        [Authorize(Roles = "admin,gerente")]
        [SwaggerOperation(Summary = "Lista todos os clientes")]
        [SwaggerResponse(statusCode: 200, description: "Dados dos clientes listados com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nao possui dados de cliente")]
        [SwaggerResponse(statusCode: 400, description: "Ocorreu um erro ao listar os clientes")]
        [EnableRateLimiting("politica_5_tentativas")]
        public IActionResult Get(int Deslocamento, int RegistroRetornado)
        {
            var resultado = _clienteUseCase.ObterTodosClientes(Deslocamento, RegistroRetornado);

            if (!resultado.IsSuccess)
            {
                if (resultado.StatusCode == StatusCodes.Status204NoContent) return NoContent();

                return StatusCode(resultado.StatusCode, resultado.Error);
            }

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter um cliente")]
        [SwaggerResponse(statusCode: 200, description: "Dados do cliente retornado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Nao encontrado dados para o cliente")]
        public IActionResult Get(int id)
        {
            var resultado = _clienteUseCase.ObterUmCliente(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Adicionar cliente")]
        public IActionResult Post(PostClienteDto model)
        {
            var resultado = _clienteUseCase.AdicionarCliente(model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Editar cliente")]
        public IActionResult Put(int id, PostClienteDto model)
        {
            var resultado = _clienteUseCase.EditarCliente(id, model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletar cliente")]
        public IActionResult Delete(int id)
        {
            var resultado = _clienteUseCase.DeletarCliente(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpGet("endereco/{cep}")]
        [SwaggerOperation(Summary = "Obter um endereco por cep")]
        [SwaggerResponse(statusCode: 200, description: "Dados do endereco retornado com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Nao encontrado dados para o cep informado")]
        public async Task<IActionResult> GetEnderecoPorCep(string cep)
        {
            var resultado = await _clienteUseCase.ObterEnderecoPorCepAsync(cep);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }
    }
}
