using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using PortalGuessr.Database;
using PortalGuessr.Services;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

builder.Services.AddDbContext<DatabaseContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
});
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<LeaderboardService>();

var app = builder.Build();

using (var sp = app.Services.CreateScope())
{
    var db = sp.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUi3(c => c.ConfigureDefaults());
app.Run();
