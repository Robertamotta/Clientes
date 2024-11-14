using Clientes.Api.Consumers;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clientes.Testes.Consumers;

public class PropostaCreditoConsumerTestes
{
    private readonly Mock<ILogger<PropostaCreditoConsumer>> logger = new();
    private readonly Mock<IPropostaCreditoServico> propostaCreditoServicoMock = new();
    private readonly PropostaCreditoConsumer consumer;

    public PropostaCreditoConsumerTestes()
    {
        consumer = new PropostaCreditoConsumer(logger.Object, propostaCreditoServicoMock.Object);
    }

    [Fact]
    public async Task PropostaCreditoConsumer_Sucesso()
    {
        // Arrange
        var mensagem = new Mock<PropostaCreditoDto>();

        mensagem.Object.Id = 1;
        mensagem.Object.AprovacaoCredito = true;

        var contextMock = new Mock<ConsumeContext<PropostaCreditoDto>>();
        contextMock.Setup(x => x.Message).Returns(mensagem.Object);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        propostaCreditoServicoMock.Verify(servico => servico.SalvarPropostaCredito(It.Is<PropostaCreditoDto>(x => x.Id == 1 && x.AprovacaoCredito == true)), Times.Once);
    }

    [Fact]
    public async Task EmissaoCartaoCreditoConsumer_Erro()
    {
        //Arrage
        var context = Mock.Of<ConsumeContext<PropostaCreditoDto>>(x => x.Message == new PropostaCreditoDto
        {
            Id = 1,
            AprovacaoCredito = true
        });

        propostaCreditoServicoMock.Setup(x => x.SalvarPropostaCredito(context.Message)).Throws<Exception>();

        //Assert
        await Assert.ThrowsAsync<Exception>(() => consumer.Consume(context));
    }
}
