namespace ConexaoSolidaria.Shared.Events;

public class DonationReceivedEvent
{
    public Guid CampaignId { get; set; }

    public decimal Amount { get; set; }

    public Guid DonorId { get; set; }

    public DateTime CreatedAt { get; set; } =
        DateTime.UtcNow;
}