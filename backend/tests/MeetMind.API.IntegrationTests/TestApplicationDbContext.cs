using MeetMind.Domain.Entities;
using MeetMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetMind.API.IntegrationTests;

public class TestApplicationDbContext : ApplicationDbContext
{
    public TestApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // pgvector and NpgsqlTsVector are not supported by the InMemory provider.
        builder.Entity<Meeting>().Ignore(m => m.Embedding);
        builder.Entity<Meeting>().Ignore("SearchVector");
    }
}
