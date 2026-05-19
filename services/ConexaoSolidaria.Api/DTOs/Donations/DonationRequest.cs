namespace ConexaoSolidaria.Api.DTOs.Donations;

public class DonationRequest
{
    public Guid CampaignId { get; set; }

    public decimal Amount { get; set; }
}