using CreditScoringEngine.Domain.Entities;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace CreditScoringEngine.Infrastructure.Messaging;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _factory = new ConnectionFactory() 
        {
            HostName = configuration["RabbitMQ:HostName"] ??"localhost"
        };

    }

    public async Task PublicarAsync<T>(T evento, string routingKey) 
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "propostas-aprovadas",
            durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evento));

        await channel.BasicPublishAsync(exchange: "",
            routingKey: "propostas-aprovadas",
            body: body);//to do, adicionar try/catch e exception do Tipo RabbitMQPublisherException(criar e registrar o handler no program.cs)

    }

}
