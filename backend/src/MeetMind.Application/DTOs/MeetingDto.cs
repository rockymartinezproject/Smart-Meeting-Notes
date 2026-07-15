using MeetMind.Domain.Enums;

namespace MeetMind.Application.DTOs;

public record MeetingDto(
    Guid Id,
    string Title,
    MeetingStatus Status,
    DateTime CreatedAt,
    int? DurationSeconds,
    long? FileSizeBytes);
