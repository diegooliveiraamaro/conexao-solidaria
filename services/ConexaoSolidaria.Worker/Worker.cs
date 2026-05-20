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

    public Worker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "donation-queue",
            durable: false,
            exclusive: false,
            autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            if (ea == null)
                return;

            var body = ea.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);          


            var donation = JsonSerializer.Deserialize<DonationReceivedEvent>(message);

            if (donation is null)
            {
                return;
            }

            using var scope = _scopeFactory.CreateScope();

            var db =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var campaign =
                await db.Campaigns.FirstOrDefaultAsync(x =>
                    x.Id == donation!.CampaignId);

            if (campaign != null)
            {
                campaign.TotalRaised += donation.Amount;

                await db.SaveChangesAsync();
            }
        };

        channel.BasicConsume(
            queue: "donation-queue",
            autoAck: true,
            consumer: consumer);

        await Task.CompletedTask;
    }
}