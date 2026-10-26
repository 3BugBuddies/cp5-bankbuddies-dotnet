using Banco.API.Domain.Entities;
using Banco.API.Domain.Errors;
using Banco.API.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationContext _context;

        public ClienteRepository(ApplicationContext context)
        {
            _context = context;
        }

        public ClienteEntity? Adicionar(ClienteEntity entity)
        {
            try
            {
                _context.Cliente.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ClienteEntity? Deletar(int Id)
        {
            try
            {
                var cliente = _context.Cliente.FirstOrDefault(x => x.Id == Id);

                if (cliente is null) throw new NoContentException("Não foi encontrado o cliente");

                _context.Cliente.Remove(cliente);
                _context.SaveChanges();

                return cliente;
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

        public ClienteEntity? Editar(int Id, ClienteEntity entity)
        {
            try
            {
                var cliente = _context.Cliente.FirstOrDefault(x => x.Id == Id);

                if (cliente is null) throw new NoContentException("Não foi encontrado o cliente");

                cliente.Nome = entity.Nome;
                cliente.Email = entity.Email;
                cliente.Cpf = entity.Cpf;
                cliente.Idade = entity.Idade;

                _context.Cliente.Update(cliente);
                _context.SaveChanges();

                return cliente;
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

        public (IEnumerable<ClienteEntity> data, int rowsCount) ObterTodos(int Deslocamento = 0, int RegistroRetornado = 30)
        {
            try
            {
                if (Deslocamento < 0) Deslocamento = 0;

                if (RegistroRetornado <= 0) RegistroRetornado = 30;

                var totalRegistros = _context.Cliente.Count();

                var resultado = _context
                    .Cliente
                    .Include(x => x.Contas)
                    .OrderBy(x => x.Id)
                    .Skip(Deslocamento)
                    .Take(RegistroRetornado)
                    .ToList();

                if (!resultado.Any()) throw new NoContentException("Não foi encontrado clientes");

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

        public ClienteEntity? ObterUm(int Id)
        {
            try
            {
                var cliente = _context
                    .Cliente
                    .Include(x => x.Contas)
                    .FirstOrDefault(x => x.Id == Id);

                if (cliente is null) throw new NoContentException("Não foi encontrado o cliente");

                return cliente;
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
