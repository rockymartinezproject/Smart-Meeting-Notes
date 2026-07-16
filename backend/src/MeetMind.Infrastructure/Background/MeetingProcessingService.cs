using MediatR;
using MeetMind.Application.Features.Meetings.Commands;
using MeetMind.Domain.Enums;
using MeetMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeetMind.Infrastructure.Background;

public class MeetingProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MeetingProcessingService> _logger;
    private readonly TimeSpan _pollInterval = TimeSpan.FromSeconds(5);

    public MeetingProcessingService(IServiceProvider serviceProvider, ILogger<MeetingProcessingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Meeting processing service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMeetingsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in meeting processing service.");
            }

            await Task.Delay(_pollInterval, stoppingToken);
        }
    }

    private async Task ProcessPendingMeetingsAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var pendingMeetingIds = await context.Meetings
            .AsNoTracking()
            .Where(m => m.Status == MeetingStatus.Uploaded)
            .OrderBy(m => m.CreatedAt)
            .Select(m => m.Id)
            .ToListAsync(stoppingToken);

        foreach (var meetingId in pendingMeetingIds)
        {
            _logger.LogInformation("Processing meeting {MeetingId}", meetingId);
            var result = await mediator.Send(new ProcessMeetingCommand(meetingId), stoppingToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to process meeting {MeetingId}: {Errors}", meetingId, string.Join(", ", result.Errors));
            }
        }
    }
}
