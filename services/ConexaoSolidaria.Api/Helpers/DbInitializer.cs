using ConexaoSolidaria.Api.Data;
using ConexaoSolidaria.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConexaoSolidaria.Api.Helpers;

public static class DbInitializer
{
    public static async Task SeedAsync(
        IServiceProvider services)
    {
        using var scope =
            services.CreateScope();

        var context =
            scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        var adminExists =
            await context.Users.AnyAsync(
                x => x.Role == "GestorONG");

        if (adminExists)
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FullName = "Administrador ONG",
            Email = "admin@ongsolidaria.com",
            CPF = "00000000000",

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    "Admin@123"),

            Role = "GestorONG"
        };

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}