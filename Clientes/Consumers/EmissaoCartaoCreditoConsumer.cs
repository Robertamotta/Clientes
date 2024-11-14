using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using MassTransit;

namespace Clientes.Api.Consumers;

public class EmissaoCartaoCreditoConsumer(ILogger<EmissaoCartaoCreditoConsumer> logger, IEmissaoCartaoCreditoServico emissaoCartaoCreditoServico) : IConsumer<EmissaoCartaoCreditoDto>
{
    public async Task Consume(ConsumeContext<EmissaoCartaoCreditoDto> context)
    {
        try
        {
            await emissaoCartaoCreditoServico.SalvarEmissaoCartaoCredito(context.Message);
        }
        catch (Exception ex)
        {
            logger.LogError($"Ocorreu um erro ao processar solicitacao. {ex.Message}");
            throw;
        }
        
    }
}
