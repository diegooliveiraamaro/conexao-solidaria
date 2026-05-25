namespace ConexaoSolidaria.Api.Entities;

public class Donation
{
    public Guid Id { get; set; }

    public string DonorName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}