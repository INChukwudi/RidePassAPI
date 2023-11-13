using HashidsNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RidePassAPI.Contracts.ServiceContracts;
using RidePassAPI.Data;
using RidePassAPI.Middleware;
using RidePassAPI.Models.IdentityModels;
using RidePassAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RidePassAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RidePassAPIContext") ?? throw new InvalidOperationException("Connection string 'RidePassAPIContext' not found.")));

builder.Services.AddDbContext<RidePassIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RidePassIdentityContext") ?? throw new InvalidOperationException("Connection string 'RidePassIdentityContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

// Identity Services
builder.Services.AddIdentityCore<AppUser>(options => { })
    .AddEntityFrameworkStores<RidePassIdentityContext>()
    .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]!)),
            ValidIssuer = builder.Configuration["Token:Issuer"]!,
            ValidateIssuer = true,
            ValidateAudience = false,
            //ValidAudience = builder.Configuration["Token:Audience"]!,
            //ValidateAudience = true,
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IHashids>(_ 
    => new Hashids(builder.Configuration["Token:Key"]!, 15));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Configuring Migrations
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<RidePassAPIContext>();
var identityContext = services.GetRequiredService<RidePassIdentityContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured while applying the migration");
}

app.Run();
