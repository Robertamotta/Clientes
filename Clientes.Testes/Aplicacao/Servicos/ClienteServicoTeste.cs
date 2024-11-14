using Clientes.Aplicacao.Servicos;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Dominio.Interfaces;
using Clientes.Infraestrutura.Interfaces;
using Moq;

namespace Clientes.Testes.Aplicacao.Servicos;

public class ClienteServicoTeste
{
    private readonly Mock<IClienteRepositorio> clienteRepositorioMock = new();
    private readonly Mock<IMensageria> mensageriaMock = new();

    private readonly ClienteServico clienteServico;

    public ClienteServicoTeste()
    {
        clienteServico = new ClienteServico(clienteRepositorioMock.Object, mensageriaMock.Object);
    }

    [Fact]
    public async Task ListarClientes_Sucesso()
    {
        // Arrange
        var clientesEsperados = new List<Cliente> { new() { Id = 1, Nome = "Cliente 1" } };
        clienteRepositorioMock.Setup(repo => repo.Listar()).ReturnsAsync(clientesEsperados);

        // Act
        var clientes = await clienteServico.ListarClientes();

        // Assert
        Assert.Equal(clientesEsperados, clientes);
    }

    [Fact]
    public async Task ObterCliente_Sucesso()
    {
        // Arrange
        var clienteEsperado = new Cliente { Id = 1, Nome = "Cliente 1" };
        clienteRepositorioMock.Setup(repo => repo.Obter(1)).ReturnsAsync(clienteEsperado);

        // Act
        var cliente = await clienteServico.ObterCliente(1);

        // Assert
        Assert.Equal(clienteEsperado, cliente);
    }

    [Fact]
    public async Task CadastrarCliente_DeveEnviarMensagem()
    {
        // Arrange
        var clienteDto = new ClienteDto { Nome = "Novo Cliente" };
        var cliente = new Cliente { Id = 1, Nome = "Novo Cliente" };
        clienteRepositorioMock.Setup(repo => repo.Cadastrar(clienteDto)).ReturnsAsync(cliente);

        // Act
        await clienteServico.CadastrarCliente(clienteDto);

        // Assert
        mensageriaMock.Verify(x => x.EnviarCadastroClienteNovo(cliente), Times.Once);
    }

    [Fact]
    public async Task AtualizarCliente_DeveChamarRepositorio()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, Nome = "Cliente Atualizado" };

        // Act
        await clienteServico.AtualizarCliente(cliente);

        // Assert
        clienteRepositorioMock.Verify(x => x.Atualizar(cliente), Times.Once);
    }

    [Fact]
    public async Task RemoverCliente_Sucesso()
    {
        // Arrange
        var clienteId = 1;

        // Act
        await clienteServico.RemoverCliente(clienteId);

        // Assert
        clienteRepositorioMock.Verify(x => x.Deletar(clienteId), Times.Once);
    }
}
