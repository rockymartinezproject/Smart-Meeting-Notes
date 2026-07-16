using MediatR;
using MeetMind.Application.Common;
using MeetMind.Application.Interfaces;
using MeetMind.Domain.Entities;
using MeetMind.Domain.Enums;

namespace MeetMind.Application.Features.Meetings.Commands;

public record ProcessMeetingCommand(Guid MeetingId) : IRequest<Result>;

public class ProcessMeetingCommandHandler : IRequestHandler<ProcessMeetingCommand, Result>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IMeetingStorageService _storageService;
    private readonly ITranscriptionService _transcriptionService;
    private readonly TimeProvider _timeProvider;

    public ProcessMeetingCommandHandler(
        IMeetingRepository meetingRepository,
        IMeetingStorageService storageService,
        ITranscriptionService transcriptionService,
        TimeProvider timeProvider)
    {
        _meetingRepository = meetingRepository;
        _storageService = storageService;
        _transcriptionService = transcriptionService;
        _timeProvider = timeProvider;
    }

    public async Task<Result> Handle(ProcessMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken);
        if (meeting is null)
        {
            return Result.Failure($"Meeting {request.MeetingId} not found.");
        }

        if (meeting.Status != MeetingStatus.Uploaded)
        {
            return Result.Failure($"Meeting {request.MeetingId} is not in Uploaded state.");
        }

        if (string.IsNullOrEmpty(meeting.FilePath))
        {
            return Result.Failure($"Meeting {request.MeetingId} has no file path.");
        }

        try
        {
            meeting.Status = MeetingStatus.Processing;
            await _meetingRepository.UpdateAsync(meeting, cancellationToken);

            await using var stream = await _storageService.GetAsync(meeting.FilePath, cancellationToken);
            var transcription = await _transcriptionService.TranscribeAsync(
                stream,
                meeting.OriginalFileName ?? "audio.mp3",
                cancellationToken);

            meeting.Status = MeetingStatus.Completed;
            meeting.DurationSeconds = (int)transcription.Duration.TotalSeconds;
            meeting.TranscriptText = transcription.FullText;
            meeting.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;

            var sequence = 0;
            meeting.TranscriptSegments = transcription.Segments.Select(s => new TranscriptSegment
            {
                Id = Guid.NewGuid(),
                MeetingId = meeting.Id,
                Sequence = sequence++,
                Text = s.Text,
                StartTime = s.Start,
                EndTime = s.End,
                Speaker = s.Speaker,
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
            }).ToList();

            await _meetingRepository.UpdateAsync(meeting, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            meeting.Status = MeetingStatus.Failed;
            meeting.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            await _meetingRepository.UpdateAsync(meeting, cancellationToken);
            return Result.Failure($"Processing failed: {ex.Message}");
        }
    }
}
