using Castle.Core.Logging;
using Clientes.Api.Consumers;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clientes.Testes.Consumers;

public class EmissaoCartaoCreditoConsumerTestes
{
    private readonly Mock<ILogger<EmissaoCartaoCreditoConsumer>> loggerMock = new();
    private readonly Mock<IEmissaoCartaoCreditoServico> emissaoCartaoCreditoServicoMock = new();
    private readonly EmissaoCartaoCreditoConsumer consumer;

    public EmissaoCartaoCreditoConsumerTestes()
    {
        consumer = new EmissaoCartaoCreditoConsumer(loggerMock.Object, emissaoCartaoCreditoServicoMock.Object);
    }

    [Fact]
    public async Task EmissaoCartaoCreditoConsumer_Sucesso()
    {
        // Arrange
        var mensagem = new Mock<EmissaoCartaoCreditoDto>();

        mensagem.Object.Id = 1;
        mensagem.Object.QtdCartoesEmitidos = 1;

        var contextMock = new Mock<ConsumeContext<EmissaoCartaoCreditoDto>>();
        contextMock.Setup(x => x.Message).Returns(mensagem.Object);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        emissaoCartaoCreditoServicoMock.Verify(servico => servico.SalvarEmissaoCartaoCredito(It.Is<EmissaoCartaoCreditoDto>(x => x.Id == 1 && x.QtdCartoesEmitidos == 1)), Times.Once);
    }

    [Fact]
    public async Task EmissaoCartaoCreditoConsumer_Erro()
    {
        //Arrage
        var context = Mock.Of<ConsumeContext<EmissaoCartaoCreditoDto>>(x => x.Message == new EmissaoCartaoCreditoDto
        {
            Id = 1,
            QtdCartoesEmitidos = 1
        });
        
        emissaoCartaoCreditoServicoMock.Setup(x => x.SalvarEmissaoCartaoCredito(context.Message)).Throws<Exception>();

        //Assert
        await Assert.ThrowsAsync<Exception>(() => consumer.Consume(context));
    }
}
