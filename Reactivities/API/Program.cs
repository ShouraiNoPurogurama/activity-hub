using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;

try
{
    var context = service.GetRequiredService<DBContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception e)
{
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occured during migration");
}

app.Run();