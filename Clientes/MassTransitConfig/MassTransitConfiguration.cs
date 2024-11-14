using Clientes.Api.Consumers;
using Clientes.Dominio.Entidades;
using MassTransit;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Text.Json;

namespace Clientes.Api.MassTransitConfig;

[ExcludeFromCodeCoverage]
public static class MassTransitConfiguration
{
    public static void ConfigureMasTransit(this IServiceCollection services)
    {
        services.ConfigurarRabbitMQ();
    }

    private static void ConfigurarRabbitMQ(this IServiceCollection services)
    {

        

        //services.AddMassTransit(x =>
        //{
        //    x.AddConsumer<PropostaCreditoConsumer>();
        //    x.AddConsumer<EmissaoCartaoCreditoConsumer>();

        //    x.UsingRabbitMq((context, cfg) =>
        //   {
        //       cfg.Host(host, port, vhost, h =>
        //       {
        //           h.Username(username);
        //           h.Password(password);
        //           if (useSsl)
        //               h.UseSsl(s => s.Protocol = SslProtocols.Tls12);
        //           h.ConfigureBatchPublish(b => b.Enabled = true);
        //           h.PublisherConfirmation = true;
        //       });

        //       //cfg.ClearSerialization();
        //       cfg.UseJsonSerializer();

        //       cfg.ConfigureJsonSerializerOptions(options =>
        //       {
        //           options.PropertyNameCaseInsensitive = true;
        //           return options;
        //       });

        //       cfg.Publish(ConfigurePublishEvento);


        //       ConfigureEndpoint<PropostaCreditoConsumer>(context, cfg, "queue.propostacredito.v1");
        //       ConfigureEndpoint<EmissaoCartaoCreditoConsumer>(context, cfg, "queue.emissaocartaocredito.v1");
        //   });
        //});
    }

    private static void ConfigureEndpoint<TConsumer>(IBusRegistrationContext context,
                                                     IRabbitMqBusFactoryConfigurator cfg,
                                                     string queue,
                                                     bool configureConsumeTopology = true,
                                                     int prefetchCount = 1, int attempts = 2)
        where TConsumer : class, IConsumer
    {
        cfg.ReceiveEndpoint(queue, e =>
        {
            e.ConfigureConsumeTopology = configureConsumeTopology;
            e.PrefetchCount = prefetchCount;
            e.UseMessageRetry(r =>
            {
                r.Interval(attempts, TimeSpan.FromSeconds(5));
            });
            e.ConfigureConsumer<TConsumer>(context);
        });
    }

    public static void ConfigurePublishEvento(IRabbitMqMessagePublishTopologyConfigurator<string> publishTopologyConfigurator)
    {
        var exchange = publishTopologyConfigurator.Exchange.ExchangeName;
        var queue = "queue.cadastrocliente.v1";
        publishTopologyConfigurator.BindQueue(exchange, queue);
    }
}
