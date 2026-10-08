using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

// Add CORS so the frontend can communicate with the backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed default roles
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Backend.Models.Role
            {
                RoleName = "Student",
                Description = "Student account"
            },
            new Backend.Models.Role
            {
                RoleName = "Administrator",
                Description = "Administrator account"
            }
        );

        db.SaveChanges();
    }
}

// Configure CORS
app.UseCors("AllowFrontend");

// OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Test endpoint
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");

// Database connection test
app.MapGet("/dbtest", async (ApplicationDbContext db) =>
{
    try
    {
        bool connected = await db.Database.CanConnectAsync();

        if (connected)
        {
            return Results.Ok(new
            {
                success = true,
                message = "Connected to rvce_db successfully."
            });
        }

        return Results.Ok(new
        {
            success = false,
            message = "Could not connect to rvce_db."
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new
        {
            success = false,
            error = ex.ToString()
        });
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}