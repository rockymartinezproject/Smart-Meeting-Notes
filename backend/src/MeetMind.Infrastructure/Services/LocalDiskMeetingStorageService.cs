using MeetMind.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace MeetMind.Infrastructure.Services;

public class LocalDiskMeetingStorageService : IMeetingStorageService
{
    private readonly string _basePath;
    private readonly ILogger<LocalDiskMeetingStorageService> _logger;

    public LocalDiskMeetingStorageService(string basePath, ILogger<LocalDiskMeetingStorageService> logger)
    {
        _basePath = basePath;
        _logger = logger;

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> SaveAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var safeFileName = Path.GetFileNameWithoutExtension(fileName)
            .Replace(" ", "_")
            .Replace(Path.DirectorySeparatorChar.ToString(), "_")
            .Replace(Path.AltDirectorySeparatorChar.ToString(), "_");

        var extension = Path.GetExtension(fileName);
        var uniqueName = $"{safeFileName}_{Guid.NewGuid():N}{extension}";
        var relativePath = $"{DateTime.UtcNow:yyyy/MM}/{uniqueName}";
        var fullPath = Path.Combine(_basePath, relativePath);

        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var destination = File.Create(fullPath);
        await fileStream.CopyToAsync(destination, cancellationToken);

        _logger.LogInformation("Saved meeting file to {Path}", fullPath);
        return relativePath;
    }

    public Task<Stream> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Meeting file not found.", fullPath);
        }

        return Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation("Deleted meeting file at {Path}", fullPath);
        }

        return Task.CompletedTask;
    }
}
