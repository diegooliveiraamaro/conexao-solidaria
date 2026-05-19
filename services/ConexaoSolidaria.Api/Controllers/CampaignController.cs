using ConexaoSolidaria.Api.Data;
using ConexaoSolidaria.Api.DTOs.Campaigns;
using ConexaoSolidaria.Api.Entities;
using ConexaoSolidaria.Api.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignController : ControllerBase
{
    private readonly AppDbContext _context;

    public CampaignController(
        AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "GestorONG")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCampaignRequest request)
    {
        if (request.EndDate < DateTime.UtcNow)
        {
            return BadRequest(new
            {
                message =
                    "Data fim não pode estar no passado."
            });
        }

        if (request.FinancialGoal <= 0)
        {
            return BadRequest(new
            {
                message =
                    "Meta financeira deve ser maior que zero."
            });
        }

        var campaign = new Campaign
        {
            Id = Guid.NewGuid(),

            Title = request.Title,

            Description = request.Description,

            StartDate = request.StartDate,

            EndDate = request.EndDate,

            FinancialGoal = request.FinancialGoal,

            TotalRaised = 0,

            Status = CampaignStatus.Ativa
        };

        _context.Campaigns.Add(campaign);

        await _context.SaveChangesAsync();

        return Ok(campaign);
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetPublic()
    {
        var campaigns = await _context.Campaigns
            .Where(x => x.Status == CampaignStatus.Ativa)
            .Select(x => new CampaignResponse
            {
                Id = x.Id,

                Title = x.Title,

                Description = x.Description,

                FinancialGoal = x.FinancialGoal,

                TotalRaised = x.TotalRaised,

                Status = x.Status.ToString()
            })
            .ToListAsync();

        return Ok(campaigns);
    }

    [Authorize(Roles = "GestorONG")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var campaigns = await _context.Campaigns
            .ToListAsync();

        return Ok(campaigns);
    }
}