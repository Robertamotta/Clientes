using Clientes.Aplicacao.Servicos;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Infraestrutura.Interfaces;
using Moq;

namespace Clientes.Testes.Aplicacao.Servicos;

public class EmissaoCartaoCreditoServicoTeste
{
    private readonly Mock<IClienteRepositorio> clienteRepositorioMock = new();
    private readonly EmissaoCartaoCreditoServico emissaoCartaoCreditoServico;

    public EmissaoCartaoCreditoServicoTeste()
    {
        emissaoCartaoCreditoServico = new EmissaoCartaoCreditoServico(clienteRepositorioMock.Object);
    }

    [Fact]
    public async Task SalvarEmissaoCartaoCredito_DeveAtualizarCliente()
    {
        // Arrange
        var emissaoCartaoCreditoDto = new EmissaoCartaoCreditoDto();
        var atualizacaoEmissaoCartaoCredito = emissaoCartaoCreditoDto.ParaAtualizacaoQtdCartoesEmitidos();
        clienteRepositorioMock.Setup(x => x.Atualizar(It.IsAny<Cliente>())).Returns(Task.CompletedTask);

        // Act
        await emissaoCartaoCreditoServico.SalvarEmissaoCartaoCredito(emissaoCartaoCreditoDto);

        // Assert
        clienteRepositorioMock.Verify(x => x.Atualizar(It.IsAny<Cliente>()), Times.Once);
    }
}
