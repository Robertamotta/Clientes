using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Clientes.Api.Consumers;

[ExcludeFromCodeCoverage]
public class EmissaoCartaoCreditoConsumer : BackgroundService
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly ILogger<EmissaoCartaoCreditoConsumer> logger;
    private readonly IServiceProvider services;

    private const string Queue = "queue.respostaemissaocartaocredito.v1";

    public EmissaoCartaoCreditoConsumer(ILogger<EmissaoCartaoCreditoConsumer> logger, IServiceProvider services)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        connection = connectionFactory.CreateConnection();

        channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: Queue,
            durable: false,
            exclusive: false,
            autoDelete: false);
        
        this.logger = logger;
        this.services = services;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            var cliente = JsonConvert.DeserializeObject<EmissaoCartaoCreditoDto>(contentString);

            if(cliente == null)
            {
                logger.LogError("Cliente recebido é nulo ou inválido");
                return;
            }

            await Complete(cliente);

            channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }
    public async Task Complete(EmissaoCartaoCreditoDto emissaoCartaoCredito)
    {
        using var scope = services.CreateScope();

        var emissaoCartaoCreditoServico = scope.ServiceProvider.GetRequiredService<IEmissaoCartaoCreditoServico>();

        await emissaoCartaoCreditoServico.SalvarEmissaoCartaoCredito(emissaoCartaoCredito);
    }
}
