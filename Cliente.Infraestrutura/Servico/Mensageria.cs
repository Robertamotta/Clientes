using Clientes.Dominio.Entidades;
using Clientes.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Clientes.Infraestrutura.Servico;

[ExcludeFromCodeCoverage]
public class Mensageria(ILogger<Mensageria> logger) : IMensageria
{
    public async Task EnviarCadastroClienteNovo(Cliente cliente)
    {
        try
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var queue = "queue.cadastrocliente.v1";
            channel.QueueDeclare(queue, false, false, false, null);

            channel.BasicPublish(exchange: string.Empty, routingKey: queue, null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cliente)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Ocorreu um erro ao postar a mensagem na fila de novos clientes");
        }
    }
}
