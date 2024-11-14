using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Clientes.Infraestrutura.Dados;

[ExcludeFromCodeCoverage]
public class ClienteRepositorio(ClientesContext context) : IClienteRepositorio
{
    public async Task<IEnumerable<Cliente>> Listar() => await context.Cliente.ToListAsync();

    public async Task<Cliente> Obter(int id) => await context.Cliente.FindAsync(id);

    public async Task<Cliente> Cadastrar(ClienteDto clienteDto)
    {
        var cliente = new Cliente
        {
            Nome = clienteDto.Nome,
            Cpf = clienteDto.Cpf,
            Email = clienteDto.Email,
            Renda = clienteDto.Renda,
            ScoreCredito = clienteDto.ScoreCredito,
            Telefone = clienteDto.Telefone
        };

        await context.Cliente.AddAsync(cliente);
        await context.SaveChangesAsync();

        return cliente;
    }

    public async Task Atualizar(Cliente cliente)
    {
        context.Cliente.Update(cliente);
        await context.SaveChangesAsync();
    }

    public async Task Deletar(int id)
    {
        var cliente = await context.Cliente.FindAsync(id);

        if (cliente != null)
        {
            context.Cliente.Remove(cliente);
            await context.SaveChangesAsync();
        }
    }
}
