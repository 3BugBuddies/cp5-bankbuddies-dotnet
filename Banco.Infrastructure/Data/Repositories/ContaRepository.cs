using Banco.API.Domain.Entities;
using Banco.API.Domain.Errors;
using Banco.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Data.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly ApplicationContext _context;

        public ContaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public ContaEntity? Adicionar(ContaEntity entity)
        {
            try
            {
                _context.Conta.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ContaEntity? Deletar(int Id)
        {
            try
            {
                var conta = _context.Conta.FirstOrDefault(x => x.Id == Id);

                if (conta is null) throw new NoContentException("Não foi encontrada a conta");

                _context.Conta.Remove(conta);
                _context.SaveChanges();

                return conta;
            }
            catch (NoContentException ex)
            {
                throw new NoContentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ContaEntity? Editar(int Id, ContaEntity entity)
        {
            try
            {
                var conta = _context.Conta.FirstOrDefault(x => x.Id == Id);

                if (conta is null) throw new NoContentException("Não foi encontrada a conta");

                conta.Agencia = entity.Agencia;
                conta.Saldo = entity.Saldo;
                conta.TipoConta = entity.TipoConta;

                _context.Conta.Update(conta);
                _context.SaveChanges();

                return conta;
            }
            catch (NoContentException ex)
            {
                throw new NoContentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public IEnumerable<ContaEntity> ObterTodasPorCliente(int clienteId)
        {
            try
            {
                var resultado = _context
                    .Conta
                    .Where(x => x.ClienteId == clienteId)
                    .ToList();

                if (!resultado.Any()) throw new NoContentException("Não foi encontrada conta para este cliente");

                return resultado;
            }
            catch (NoContentException ex)
            {
                throw new NoContentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public (IEnumerable<ContaEntity> data, int rowsCount) ObterTodos(int Deslocamento = 0, int RegistroRetornado = 30)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;

                if (RegistroRetornado <= 0) RegistroRetornado = 30;

                var totalRegistros = _context.Conta.Count();

                var resultado = _context
                    .Conta
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToList();

                if (!resultado.Any()) throw new NoContentException("Não foi encontrada contas");

                return (resultado, totalRegistros);
            }
            catch (NoContentException ex)
            {
                throw new NoContentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ContaEntity? ObterUma(int Id)
        {
            try
            {
                var conta = _context
                    .Conta
                    .FirstOrDefault(x => x.Id == Id);

                if (conta is null) throw new NoContentException("Não foi encontrada a conta");

                return conta;
            }
            catch (NoContentException ex)
            {
                throw new NoContentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
