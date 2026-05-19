namespace ConexaoSolidaria.Api.DTOs.Campaigns;

public class CampaignResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal FinancialGoal { get; set; }

    public decimal TotalRaised { get; set; }

    public string Status { get; set; } = string.Empty;
}