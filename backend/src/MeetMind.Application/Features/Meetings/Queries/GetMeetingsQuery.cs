using MediatR;
using MeetMind.Domain.Entities;

namespace MeetMind.Application.Features.Meetings.Queries;

public record GetMeetingsQuery : IRequest<List<Meeting>>;

public class GetMeetingsQueryHandler : IRequestHandler<GetMeetingsQuery, List<Meeting>>
{
    public Task<List<Meeting>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
    {
        // TODO: wire up repository / DbContext on Day 2
        return Task.FromResult(new List<Meeting>());
    }
}
