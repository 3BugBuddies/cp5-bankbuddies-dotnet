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
    public class ContaUseCase : IContaUseCase
    {
        private readonly ILogger<ContaUseCase> _logger;
        private readonly IContaRepository _contaRepository;

        public ContaUseCase(IContaRepository contaRepository, ILogger<ContaUseCase> logger)
        {
            _contaRepository = contaRepository;
            _logger = logger;
        }

        public OperationResult<ContaEntity?> AdicionarConta(PostContaDto entity)
        {
            try
            {
                var result = _contaRepository.Adicionar(entity.ToContaEntity());

                return OperationResult<ContaEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return OperationResult<ContaEntity?>.Failure("Ocorreu um erro ao salvar a conta", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<ContaEntity?> DeletarConta(int Id)
        {
            try
            {
                var result = _contaRepository.Deletar(Id);

                return OperationResult<ContaEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<ContaEntity?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<ContaEntity?>.Failure("Ocorreu um erro ao deletar a conta", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<ContaEntity?> EditarConta(int Id, PostContaDto entity)
        {
            try
            {
                var result = _contaRepository.Editar(Id, entity.ToContaEntity());

                return OperationResult<ContaEntity?>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<ContaEntity?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<ContaEntity?>.Failure("Ocorreu um erro ao editar a conta", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<IEnumerable<ContaEntity>> ObterContasPorCliente(int clienteId)
        {
            try
            {
                _logger.LogInformation("ObterContasPorCliente iniciado na application");
                var result = _contaRepository.ObterTodasPorCliente(clienteId);

                return OperationResult<IEnumerable<ContaEntity>>.Success(result, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                return OperationResult<IEnumerable<ContaEntity>>.Failure(ex.Message, (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationResult<IEnumerable<ContaEntity>>.Failure("Ocorreu um erro ao buscar as contas do cliente", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<PageResponseDto<GetContaDto>> ObterTodasContas(int Deslocamento, int RegistroRetornado)
        {
            try
            {
                _logger.LogInformation("ObterTodasContas iniciado na application");
                var result = _contaRepository.ObterTodos(Deslocamento, RegistroRetornado);

                var pageResponse = new PageResponseDto<GetContaDto>
                {
                    Items = result.data.Select(item => item.ToContaDto(new List<LinkDto>
                    {
                        new LinkDto($"/api/conta/{item.Id}", "get_uma_conta", "GET"),
                        new LinkDto($"/api/conta/{item.Id}", "update_conta", "PUT"),
                        new LinkDto($"/api/conta/{item.Id}", "delete_conta", "DELETE"),
                    })),
                    Deslocamento = Deslocamento,
                    RegistroRetornado = RegistroRetornado,
                    TotalRegistros = result.rowsCount,
                    Links = new List<LinkDto>
                    {
                        new LinkDto($"/api/conta?Deslocamento={Deslocamento}&RegistroRetornado={RegistroRetornado}", "self", "GET"),
                        new LinkDto("/api/conta", "create_conta", "POST")
                    }
                };

                return OperationResult<PageResponseDto<GetContaDto>>.Success(pageResponse, (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                _logger.LogWarning("Nao retornou nenhuma informação do ObterTodasContas - Parametros: D: {0} - R: {1}", Deslocamento, RegistroRetornado);
                return OperationResult<PageResponseDto<GetContaDto>>.Failure(ex.Message, (int)HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] - {0}", ex.Message);
                return OperationResult<PageResponseDto<GetContaDto>>.Failure("Ocorreu um erro ao buscar todas as contas", (int)HttpStatusCode.BadRequest);
            }
        }

        public OperationResult<GetContaDto?> ObterUmaConta(int Id)
        {
            try
            {
                _logger.LogInformation("ObterUmaConta iniciado na application");
                var result = _contaRepository.ObterUma(Id);

                var links = new List<LinkDto>
                {
                    new LinkDto($"/api/conta/{Id}", "get_uma_conta", "GET"),
                    new LinkDto($"/api/conta/{Id}", "update_conta", "PUT"),
                    new LinkDto($"/api/conta/{Id}", "delete_conta", "DELETE"),
                };

                return OperationResult<GetContaDto?>.Success(result?.ToContaDto(links), (int)HttpStatusCode.OK);
            }
            catch (NoContentException ex)
            {
                _logger.LogWarning("Nao retornou nenhuma informação do ObterUmaConta - Parametros: ID: {0}", Id);
                return OperationResult<GetContaDto?>.Failure(ex.Message, (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR] - {0}", ex.Message);
                return OperationResult<GetContaDto?>.Failure("Ocorreu um erro ao obter uma conta", (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
