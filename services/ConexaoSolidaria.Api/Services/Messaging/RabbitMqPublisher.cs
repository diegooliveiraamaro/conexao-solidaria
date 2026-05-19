using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ConexaoSolidaria.Api.Services.Messaging;

public class RabbitMqPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Publish<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName =
                _configuration["RabbitMQ:Host"]
        };

        using var connection =
            factory.CreateConnection();

        using var channel =
            connection.CreateModel();

        channel.QueueDeclare(
            queue: "donation.process",
            durable: false,
            exclusive: false,
            autoDelete: false);

        var body =
            Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: "",
            routingKey: "donation.process",
            body: body);
    }
}