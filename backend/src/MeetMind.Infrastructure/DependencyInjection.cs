using MeetMind.Application.Interfaces;
using MeetMind.Domain.Entities;
using MeetMind.Domain.Interfaces;
using MeetMind.Infrastructure.Data;
using MeetMind.Infrastructure.Identity;
using MeetMind.Infrastructure.Repositories;
using MeetMind.Infrastructure.Services;
using MeetMind.Infrastructure.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pgvector.EntityFrameworkCore;

namespace MeetMind.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.MigrationsAssembly("MeetMind.Infrastructure");
                    b.UseVector();
                }));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IMeetingRepository, MeetingRepository>();

        services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.SectionName));
        var storageOptions = configuration.GetSection(StorageOptions.SectionName).Get<StorageOptions>() ?? new StorageOptions();
        var uploadPath = Path.GetFullPath(storageOptions.LocalDiskPath);
        services.AddSingleton<IMeetingStorageService>(provider =>
            new LocalDiskMeetingStorageService(
                uploadPath,
                provider.GetRequiredService<ILogger<LocalDiskMeetingStorageService>>()));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
