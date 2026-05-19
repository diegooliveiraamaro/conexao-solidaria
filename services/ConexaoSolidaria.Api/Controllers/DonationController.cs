using System.Security.Claims;
using ConexaoSolidaria.Api.Data;
using ConexaoSolidaria.Api.DTOs.Donations;
using ConexaoSolidaria.Api.Enums;
using ConexaoSolidaria.Api.Services.Messaging;
using ConexaoSolidaria.Shared.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationController : ControllerBase
{
    private readonly AppDbContext _context;

    private readonly RabbitMqPublisher _publisher;

    public DonationController(
        AppDbContext context,
        RabbitMqPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Donate(
        DonationRequest request)
    {
        var campaign =
            await _context.Campaigns
                .FirstOrDefaultAsync(
                    x => x.Id == request.CampaignId);

        if (campaign is null)
        {
            return NotFound(new
            {
                message = "Campanha não encontrada."
            });
        }

        if (campaign.Status != CampaignStatus.Ativa)
        {
            return BadRequest(new
            {
                message =
                    "Campanha encerrada ou cancelada."
            });
        }

        var donorId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        var donationEvent =
            new DonationReceivedEvent
            {
                CampaignId = request.CampaignId,

                Amount = request.Amount,

                DonorId = Guid.Parse(donorId!)
            };

        _publisher.Publish(donationEvent);

        return Accepted(new
        {
            message =
                "Doação enviada para processamento."
        });
    }
}