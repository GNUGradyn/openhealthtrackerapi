using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.ClientId = builder.Configuration.GetValue<string>("IdentityStore:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("IdentityStore:ClientSecret");
        options.Authority = builder.Configuration.GetValue<string>("IdentityStore:Authority");
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.SaveTokens = true;
        options.ResponseType = OpenIdConnectResponseType.Code;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

// OIDC


app.Run();