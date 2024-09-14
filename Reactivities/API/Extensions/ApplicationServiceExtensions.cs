﻿using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Application.Photos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
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
                policy => { policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000"); });
        });

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(List.Handler).Assembly));
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Create>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IPhotoAccessor, PhotoAccessor>();
        services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
        
        return services;
    }
}