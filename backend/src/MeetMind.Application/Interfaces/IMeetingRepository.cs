using MeetMind.Domain.Entities;

namespace MeetMind.Application.Interfaces;

public interface IMeetingRepository
{
    Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Meeting>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task AddAsync(Meeting meeting, CancellationToken cancellationToken = default);
    Task UpdateAsync(Meeting meeting, CancellationToken cancellationToken = default);
    Task DeleteAsync(Meeting meeting, CancellationToken cancellationToken = default);
}
