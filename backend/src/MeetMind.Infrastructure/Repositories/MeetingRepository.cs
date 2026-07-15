using MeetMind.Application.Interfaces;
using MeetMind.Domain.Entities;
using MeetMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetMind.Infrastructure.Repositories;

public class MeetingRepository : IMeetingRepository
{
    private readonly ApplicationDbContext _context;

    public MeetingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Meetings
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<IReadOnlyList<Meeting>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return _context.Meetings
            .AsNoTracking()
            .Where(m => m.OwnerId == ownerId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => (IReadOnlyList<Meeting>)t.Result, cancellationToken);
    }

    public async Task AddAsync(Meeting meeting, CancellationToken cancellationToken = default)
    {
        await _context.Meetings.AddAsync(meeting, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Meeting meeting, CancellationToken cancellationToken = default)
    {
        _context.Meetings.Update(meeting);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Meeting meeting, CancellationToken cancellationToken = default)
    {
        _context.Meetings.Remove(meeting);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
