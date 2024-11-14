using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Dominio.Interfaces;
using Clientes.Infraestrutura.Interfaces;

namespace Clientes.Aplicacao.Servicos;

public class ClienteServico(IClienteRepositorio clienteRepositorio, IMensageria mensageria) : IClienteServico
{
    public async Task<IEnumerable<Cliente>> ListarClientes()
    {
        return await clienteRepositorio.Listar();
    } 

    public async Task<Cliente> ObterCliente(int id)
    {
        return await clienteRepositorio.Obter(id);
    }

    public async Task CadastrarCliente(ClienteDto clienteDto)
    {
        var cliente = await clienteRepositorio.Cadastrar(clienteDto);
        await mensageria.EnviarCadastroClienteNovo(cliente);
    }

    public async Task AtualizarCliente(Cliente cliente)
    {
        await clienteRepositorio.Atualizar(cliente);
    }

    public async Task RemoverCliente(int id)
    {
        await clienteRepositorio.Deletar(id);
    }
}
