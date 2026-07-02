using QuestPDF.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TreningIRazvoj.API.Middleware;
using TreningIRazvoj.API.Podesavanja;
using TreningIRazvoj.API.Ponasanja;
using TreningIRazvoj.API.Servisi;
using TreningIRazvoj.API.Validatori.Zaposleni;
using TreningIRazvoj.Domen.Interfejsi;
using TreningIRazvoj.Infrastruktura.Identitet;
using TreningIRazvoj.Infrastruktura.Podaci;
using TreningIRazvoj.Infrastruktura.Repozitorijumi;


var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// JWT podešavanja
builder.Services.Configure<JwtPodesavanja>(
    builder.Configuration.GetSection("Jwt"));

var jwtPodesavanja = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtPodesavanja>()
    ?? throw new InvalidOperationException(
        "JWT podešavanja nisu pronađena.");

// Baza podataka
builder.Services.AddDbContext<TreningIRazvojKontekst>(opcije =>
    opcije.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "PodrazumevanaKonekcija")));

// Identity
builder.Services
    .AddIdentity<Korisnik, IdentityRole>()
    .AddEntityFrameworkStores<TreningIRazvojKontekst>()
    .AddDefaultTokenProviders();

// JWT autentifikacija
builder.Services
    .AddAuthentication(opcije =>
    {
        opcije.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;

        opcije.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        opcije.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opcije =>
    {
        opcije.RequireHttpsMetadata = false;
        opcije.SaveToken = true;

        opcije.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtPodesavanja.Izdavalac,

                ValidateAudience = true,
                ValidAudience = jwtPodesavanja.Primalac,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        jwtPodesavanja.Kljuc)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };

        opcije.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = kontekst =>
            {
                Console.WriteLine(
                    "JWT GREŠKA: " +
                    kontekst.Exception.Message);

                return Task.CompletedTask;
            }
        };
    });

// Autorizacija
builder.Services.AddAuthorization();

// Servisi
builder.Services.AddScoped<IJedinicaRada, JedinicaRada>();
builder.Services.AddScoped<IJwtServis, JwtServis>();
builder.Services.AddScoped<IIzvestajServis, IzvestajServis>();

// Kontroleri
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// MediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Program>();

    config.AddOpenBehavior(
        typeof(ValidacionoPonasanje<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<
    KreirajZaposlenogValidator>();

var app = builder.Build();

// OpenAPI samo u razvojnom okruženju
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Globalna obrada grešaka
app.UseMiddleware<GlobalnaObradaIzuzetaka>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();