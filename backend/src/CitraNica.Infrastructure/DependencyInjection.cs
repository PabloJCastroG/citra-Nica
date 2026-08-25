using CitraNica.Application.Interfaces;
using CitraNica.Domain.Entities;
using CitraNica.Infrastructure.Authentication;
using CitraNica.Infrastructure.Catalog;
using CitraNica.Infrastructure.Chat;
using CitraNica.Infrastructure.Farms;
using CitraNica.Infrastructure.Orders;
using CitraNica.Infrastructure.Persistence;
using CitraNica.Infrastructure.Publications;
using CitraNica.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CitraNica.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CitraNica")
            ?? throw new InvalidOperationException(
                "La cadena de conexión 'CitraNica' no está configurada.");

        services.AddDbContext<CitraNicaDbContext>(options =>
            options
                .UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString))
                .UseSnakeCaseNamingConvention());

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));
        services.AddScoped<PasswordHasher<Usuario>>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ICatalogoService, CatalogoService>();
        services.AddScoped<IFincaService, FincaService>();
        services.AddScoped<IPublicacionService, PublicacionService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
