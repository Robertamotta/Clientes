using Clientes.Aplicacao.Servicos;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Infraestrutura.Interfaces;
using Moq;

namespace Clientes.Testes.Aplicacao.Servicos;

public class PropostaCreditoServicoTeste
{
    private readonly Mock<IClienteRepositorio> clienteRepositorioMock = new();
    private readonly PropostaCreditoServico propostaCreditoServico;

    public PropostaCreditoServicoTeste()
    {
        propostaCreditoServico = new PropostaCreditoServico(clienteRepositorioMock.Object);
    }

    [Fact]
    public async Task SalvarEmissaoCartaoCredito_DeveAtualizarCliente()
    {
        // Arrange
        var propostaCreditoDto = new PropostaCreditoDto();
        var atualizacaoEmissaoCartaoCredito = propostaCreditoDto.ParaAtualizacaoPropostaCredito();
        clienteRepositorioMock.Setup(x => x.Atualizar(It.IsAny<Cliente>())).Returns(Task.CompletedTask);

        // Act
        await propostaCreditoServico.SalvarPropostaCredito(propostaCreditoDto);

        // Assert
        clienteRepositorioMock.Verify(x => x.Atualizar(It.IsAny<Cliente>()), Times.Once);
    }
}
