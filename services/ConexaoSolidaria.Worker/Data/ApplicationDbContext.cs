using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ConexaoSolidaria.Worker.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
}

public class Campaign
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal GoalAmount { get; set; }

    public decimal TotalRaised { get; set; }
}