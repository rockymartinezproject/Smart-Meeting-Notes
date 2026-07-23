using System.Net.Http.Headers;
using MeetMind.API;
using MeetMind.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MeetMind.API.IntegrationTests;

public class MeetMindApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.Testing.json", optional: false);
        });

        builder.ConfigureServices(services =>
        {
            // Remove the production Npgsql DbContext registration.
            var descriptors = services.Where(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
                    || d.ServiceType == typeof(ApplicationDbContext)
                    || (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(IDbContextOptionsConfiguration<>)))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("meetmind_test"));

            // Use the test context that ignores pgvector-specific properties.
            services.AddScoped<ApplicationDbContext>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
                return new TestApplicationDbContext(options);
            });
        });
    }
}
