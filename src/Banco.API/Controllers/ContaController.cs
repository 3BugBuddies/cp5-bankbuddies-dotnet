using Banco.API.Application.Dtos;
using Banco.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Banco.API.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly IContaUseCase _contaUseCase;

        public ContaController(IContaUseCase contaUseCase)
        {
            _contaUseCase = contaUseCase;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as contas")]
        [SwaggerResponse(statusCode: 200, description: "Dados das contas listadas com sucesso")]
        [SwaggerResponse(statusCode: 204, description: "Nao possui dados de conta")]
        public IActionResult Get(int Deslocamento, int RegistroRetornado)
        {
            var resultado = _contaUseCase.ObterTodasContas(Deslocamento, RegistroRetornado);

            if (!resultado.IsSuccess)
            {
                if (resultado.StatusCode == StatusCodes.Status204NoContent) return NoContent();

                return StatusCode(resultado.StatusCode, resultado.Error);
            }

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter uma conta")]
        public IActionResult Get(int id)
        {
            var resultado = _contaUseCase.ObterUmaConta(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpGet("cliente/{clienteId}")]
        [SwaggerOperation(Summary = "Lista as contas de um cliente")]
        public IActionResult GetPorCliente(int clienteId)
        {
            var resultado = _contaUseCase.ObterContasPorCliente(clienteId);

            if (!resultado.IsSuccess)
            {
                if (resultado.StatusCode == StatusCodes.Status204NoContent) return NoContent();

                return StatusCode(resultado.StatusCode, resultado.Error);
            }

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Adicionar conta")]
        public IActionResult Post(PostContaDto model)
        {
            var resultado = _contaUseCase.AdicionarConta(model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,gerente")]
        [SwaggerOperation(Summary = "Editar conta")]
        public IActionResult Put(int id, PostContaDto model)
        {
            var resultado = _contaUseCase.EditarConta(id, model);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Deletar conta")]
        public IActionResult Delete(int id)
        {
            var resultado = _contaUseCase.DeletarConta(id);

            if (!resultado.IsSuccess) return StatusCode(resultado.StatusCode, resultado.Error);

            return StatusCode(resultado.StatusCode, resultado.Value);
        }
    }
}
