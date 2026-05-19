using ConexaoSolidaria.Api.Data;
using ConexaoSolidaria.Api.DTOs.Auth;
using ConexaoSolidaria.Api.Entities;
using ConexaoSolidaria.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        AppDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var emailExists = await _context.Users
            .AnyAsync(x => x.Email == request.Email);

        if (emailExists)
        {
            return BadRequest(new
            {
                message = "E-mail já cadastrado."
            });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            CPF = request.CPF,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password),

            Role = "Doador"
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Usuário criado com sucesso."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Usuário ou senha inválidos."
            });
        }

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!passwordValid)
        {
            return Unauthorized(new
            {
                message = "Usuário ou senha inválidos."
            });
        }

        var token =
            _jwtService.GenerateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        });
    }
}