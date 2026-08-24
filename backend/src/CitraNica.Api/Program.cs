using System.Text;
using System.Text.Json.Serialization;
using CitraNica.Infrastructure;
using CitraNica.Infrastructure.Authentication;
using CitraNica.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
const string developmentJwtKey =
    "CHANGE_THIS_DEVELOPMENT_KEY_32_CHARS_MINIMUM";

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException("La configuración JWT no existe.");

if (string.IsNullOrWhiteSpace(jwtOptions.Key)
    || Encoding.UTF8.GetByteCount(jwtOptions.Key) < 32
    || (!builder.Environment.IsDevelopment()
        && jwtOptions.Key == developmentJwtKey))
{
    throw new InvalidOperationException(
        "La clave JWT debe configurarse de forma privada y tener "
        + "al menos 32 caracteres.");
}

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CitraNica API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT obtenido al iniciar sesión."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        }] = Array.Empty<string>()
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapGet(
    "/health/database",
    async (CitraNicaDbContext dbContext, CancellationToken cancellationToken) =>
    {
        var canConnect = await dbContext.Database
            .CanConnectAsync(cancellationToken);

        return canConnect
            ? Results.Ok(new { status = "ok", database = "connected" })
            : Results.Problem(
                title: "No se pudo conectar con MySQL.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
    });

app.Run();
