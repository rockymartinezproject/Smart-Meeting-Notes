using MediatR;
using MeetMind.Application.DTOs;
using MeetMind.Application.Interfaces;
using MeetMind.Domain.Interfaces;

namespace MeetMind.Application.Features.Meetings.Queries;

public record GetMyMeetingsQuery : IRequest<IReadOnlyList<MeetingDto>>;

public class GetMyMeetingsQueryHandler : IRequestHandler<GetMyMeetingsQuery, IReadOnlyList<MeetingDto>>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyMeetingsQueryHandler(IMeetingRepository meetingRepository, ICurrentUserService currentUserService)
    {
        _meetingRepository = meetingRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<MeetingDto>> Handle(GetMyMeetingsQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return new List<MeetingDto>();
        }

        var meetings = await _meetingRepository.GetByOwnerAsync(_currentUserService.UserId.Value, cancellationToken);

        return meetings.Select(m => new MeetingDto(
            m.Id,
            m.Title,
            m.Status,
            m.CreatedAt,
            m.DurationSeconds,
            m.FileSizeBytes)).ToList();
    }
}
