using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;

namespace Clientes.Infraestrutura.Interfaces;

public interface IClienteRepositorio
{
    Task<IEnumerable<Cliente>> Listar();
    Task<Cliente> Obter(int id);
    Task<Cliente> Cadastrar(ClienteDto clienteDto);
    Task Atualizar(Cliente cliente);
    Task Deletar (int id);
}
