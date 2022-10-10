﻿using AppCitas.Service.Data;
using AppCitas.Service.Helpers;
using AppCitas.Service.Interfaces;
using AppCitas.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IphotoService, PhotoService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

}
