using Banco.API.Application.Dtos;
using Banco.API.Application.Interfaces;
using Banco.API.Application.Mappers;
using Banco.API.Domain.Entities;
using Banco.API.Domain.Errors;
using Banco.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Banco.API.Application.UseCases
{
    public class ClienteUseCase : IClienteUseCase
    {
        private readonly ILogger<ClienteUseCase> _logger;
        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteService _clienteService;

        public ClienteUseCase(IClienteRepository clienteRepository, ILogger<ClienteUseCase> logger, IClienteService clienteService)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
            _clienteService = clienteService;
        }

        public OperationResult<ClienteEntity?> AdicionarCliente(PostClienteDto entity)
        {
            try
            {
                var result = _clienteRepository.Adicionar(entity.ToClienteEntity());

                return OperationResult<ClienteEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return OperationResult<ClienteEntity?>.Failure("Ocorreu um erro ao salvar o cliente", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<ClienteEntity?> DeletarCliente(int Id)
        {
            try
            {
                var result = _clienteRepository.Deletar(Id);

                return OperationResult<ClienteEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<ClienteEntity?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<ClienteEntity?>.Failure("Ocorreu um erro ao deletar o cliente", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<ClienteEntity?> EditarCliente(int Id, PostClienteDto entity)
        {
            try
            {
                var result = _clienteRepository.Editar(Id, entity.ToClienteEntity());

                return OperationResult<ClienteEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<ClienteEntity?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<ClienteEntity?>.Failure("Ocorreu um erro ao editar o cliente", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<PageResponseDto<GetClienteDto>> ObterTodosClientes(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("ObterTodosClientes iniciado na application");
                var result = _clienteRepository.ObterTodos(Deslocamento, RegistroRetornado);

                var pageResponse = new PageResponseDto<GetClienteDto>
                {
                    Items = result.data.Select(item => item.ToClienteDto(new List<LinkDto>
                    {
                        new LinkDto($"/api/cliente/{item.Id}", "get_um_cliente", "GET"),
                        new LinkDto($"/api/cliente/{item.Id}", "update_cliente", "PUT"),
                        new LinkDto($"/api/cliente/{item.Id}", "delete_cliente", "DELETE"),
                    })),
                    Deslocamento = Deslocamento,
                    RegistroRetornado = RegistroRetornado,
                    TotalRegistros = result.rowsCount,
                    Links = new List<LinkDto>
                    {
                        new LinkDto($"/api/cliente?Deslocamento={Deslocamento}&RegistroRetornado={RegistroRetornado}", "self", "GET"),
                        new LinkDto("/api/cliente", "create_cliente", "POST")
                    }
                };

                return OperationResult<PageResponseDto<GetClienteDto>>.Success(pageResponse, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                _logger.LogWarning("Nao retornou nenhuma informação do ObterTodosClientes - Parametros: D: {0} - R: {1}", Deslocamento, RegistroRetornado);
                return OperationResult<PageResponseDto<GetClienteDto>>.Failure(ex.Message, (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] - {0}", ex.Message);
                return OperationResult<PageResponseDto<GetClienteDto>>.Failure("Ocorreu um erro ao buscar todos os clientes", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<GetClienteDto?> ObterUmCliente(int Id)
        {
            try
            {
                _logger.LogInformation("ObterUmCliente iniciado na application");
                var result = _clienteRepository.ObterUm(Id);

                var links = new List<LinkDto>
                {
                    new LinkDto($"/api/cliente/{Id}", "get_um_cliente", "GET"),
                    new LinkDto($"/api/cliente/{Id}", "update_cliente", "PUT"),
                    new LinkDto($"/api/cliente/{Id}", "delete_cliente", "DELETE"),
                };

                return OperationResult<GetClienteDto?>.Success(result?.ToClienteDto(links), (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                _logger.LogWarning("Nao retornou nenhuma informação do ObterUmCliente - Parametros: ID: {0}", Id);
                return OperationResult<GetClienteDto?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] - {0}", ex.Message);
                return OperationResult<GetClienteDto?>.Failure("Ocorreu um erro ao obter um cliente", (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task<OperationResult<EnderecoModel?>> ObterEnderecoPorCepAsync(string cep)
        {
            try
            {
                var result = await _clienteService.ObterEnderecoPorCepAsync(cep);

                return OperationResult<EnderecoModel?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<EnderecoModel?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<EnderecoModel?>.Failure("Ocorreu um erro ao buscar o cep", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
