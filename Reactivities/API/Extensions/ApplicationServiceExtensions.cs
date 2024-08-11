using Application.Activities;
using Application.Core;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        // Add services to the container.

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DBContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"))
        );

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy",
                policy => { policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"); });
        });

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(List.Handler).Assembly));
        return services; 
    }
}