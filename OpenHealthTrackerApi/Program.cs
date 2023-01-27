using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenHealthTrackerApi.Services.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Import JWT certificate
var cert = new X509Certificate2(
    Convert.FromBase64String(builder.Configuration.GetValue<string>("IdentityStore:IssuerCert")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IActivityDbService, ActivityDbService>();
builder.Services.AddScoped<IEmotionDbService, EmotionDbService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetValue<string>("IdentityStore:Authority"),
            IssuerSigningKey = new X509SecurityKey(cert), // https://stackoverflow.com/questions/46294373/net-core-issuersigningkey-from-file-for-jwt-bearer-authentication
            ValidAudience = builder.Configuration["IdentityStore:Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true, // JWTs are required to have "exp" property set
            ValidateLifetime = true, // the "exp" will be validated
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();