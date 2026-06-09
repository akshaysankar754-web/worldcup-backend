using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IResultService, ResultService>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key_world_cup_polling_system_2026_very_long_for_hmac_sha256";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

// Seed Database on startup if needed (Safe approach for containers)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Make sure database is created/migrated
    // context.Database.Migrate(); // Optional: Automatically apply migrations on startup. Assuming manual migration or done here.
    
    // Ensure Admin user exists
    context.Database.EnsureCreated(); // Creates DB if not exists. Can be used instead of Migrations for simple scenarios.
    
    // SAFE DB PATCH FOR EXISTING DATABASES
    try {
        context.Database.ExecuteSqlRaw("ALTER TABLE `Settings` ADD COLUMN `PollOpen` tinyint(1) NOT NULL DEFAULT 0;");
    } catch { } // Ignore if already exists
    
    if (!context.Users.Any(u => u.Role == "Admin"))
    {
        context.Users.Add(new Backend.Models.User
        {
            Name = "Admin",
            Email = "admin@worldcup.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
