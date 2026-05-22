using System.Text;
using System.Text.Json;
using ConexaoSolidaria.Shared.Events;
using ConexaoSolidaria.Worker.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConexaoSolidaria.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IModel? _channel;

    public Worker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        // RETRY até RabbitMQ ficar pronto
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Tentando conectar no RabbitMQ...");

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                Console.WriteLine("Conectado ao RabbitMQ.");

                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ indisponível: {ex.Message}");

                await Task.Delay(5000, stoppingToken);
            }
        }

        if (_channel == null)
            return;

        _channel.QueueDeclare(
            queue: "donation-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Mensagem recebida: {message}");

                var donation =
                    JsonSerializer.Deserialize<DonationReceivedEvent>(message);

                if (donation == null)
                    return;

                using var scope = _scopeFactory.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                var campaign =
                    await db.Campaigns.FirstOrDefaultAsync(x =>
                        x.Id == donation.CampaignId);

                if (campaign != null)
                {
                    campaign.TotalRaised += donation.Amount;

                    await db.SaveChangesAsync();

                    Console.WriteLine("Campanha atualizada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: "donation-queue",
            autoAck: true,
            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();

        base.Dispose();
    }
}