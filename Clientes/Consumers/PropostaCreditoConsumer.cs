using Clientes.Dominio.DTOs;
using Clientes.Dominio.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Clientes.Api.Consumers;

[ExcludeFromCodeCoverage]
public class PropostaCreditoConsumer : BackgroundService
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly IServiceProvider services;
    private readonly ILogger<PropostaCreditoConsumer> logger;

    private const string Queue = "queue.respostapropostacredito.v1";

    public PropostaCreditoConsumer(ILogger<PropostaCreditoConsumer> logger, IServiceProvider services)
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
            var cliente = JsonConvert.DeserializeObject<PropostaCreditoDto>(contentString);

            if (cliente == null)
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

    public async Task Complete(PropostaCreditoDto propostaCredito)
    {
        using var scope = services.CreateScope();

        var propostaCreditoServico = scope.ServiceProvider.GetRequiredService<IPropostaCreditoServico>();

        await propostaCreditoServico.SalvarPropostaCredito(propostaCredito);
    }
}
