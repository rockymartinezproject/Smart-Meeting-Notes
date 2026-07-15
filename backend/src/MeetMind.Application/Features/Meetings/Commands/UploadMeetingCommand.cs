using MediatR;
using MeetMind.Application.Common;
using MeetMind.Application.DTOs;
using MeetMind.Application.Interfaces;
using MeetMind.Domain.Entities;
using MeetMind.Domain.Enums;
using MeetMind.Domain.Interfaces;

namespace MeetMind.Application.Features.Meetings.Commands;

public record UploadMeetingCommand(Stream FileStream, string FileName, string ContentType, long FileSize)
    : IRequest<Result<MeetingDto>>;

public class UploadMeetingCommandHandler : IRequestHandler<UploadMeetingCommand, Result<MeetingDto>>
{
    private readonly IMeetingStorageService _storageService;
    private readonly IMeetingRepository _meetingRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly TimeProvider _timeProvider;

    public UploadMeetingCommandHandler(
        IMeetingStorageService storageService,
        IMeetingRepository meetingRepository,
        ICurrentUserService currentUserService,
        TimeProvider timeProvider)
    {
        _storageService = storageService;
        _meetingRepository = meetingRepository;
        _currentUserService = currentUserService;
        _timeProvider = timeProvider;
    }

    public async Task<Result<MeetingDto>> Handle(UploadMeetingCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            return Result<MeetingDto>.Failure("User is not authenticated.");
        }

        var allowedTypes = new[] { "audio/mpeg", "audio/wav", "audio/x-wav", "audio/mp4", "audio/m4a", "audio/x-m4a" };
        var extension = Path.GetExtension(request.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".mp3", ".wav", ".m4a" };

        if (!allowedExtensions.Contains(extension) || !allowedTypes.Contains(request.ContentType.ToLowerInvariant()))
        {
            return Result<MeetingDto>.Failure("Only MP3, WAV, and M4A audio files are allowed.");
        }

        // Generic dev limit: 500 MB. Plan-specific limits will be enforced on Day 23.
        const long maxFileSize = 500L * 1024 * 1024;
        if (request.FileSize > maxFileSize)
        {
            return Result<MeetingDto>.Failure("File size exceeds the maximum allowed limit of 500 MB.");
        }

        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            Title = Path.GetFileNameWithoutExtension(request.FileName),
            OriginalFileName = request.FileName,
            FileSizeBytes = request.FileSize,
            Status = MeetingStatus.Uploading,
            OwnerId = _currentUserService.UserId.Value,
            CreatedAt = _timeProvider.GetUtcNow().UtcDateTime
        };

        // Save file to storage
        var storedPath = await _storageService.SaveAsync(request.FileStream, request.FileName, request.ContentType, cancellationToken);
        meeting.FilePath = storedPath;
        meeting.Status = MeetingStatus.Uploaded;

        await _meetingRepository.AddAsync(meeting, cancellationToken);

        var dto = new MeetingDto(
            meeting.Id,
            meeting.Title,
            meeting.Status,
            meeting.CreatedAt,
            meeting.DurationSeconds,
            meeting.FileSizeBytes);

        return Result<MeetingDto>.Success(dto);
    }
}
