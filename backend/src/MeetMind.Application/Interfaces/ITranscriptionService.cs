namespace MeetMind.Application.Interfaces;

public record TranscriptSegmentDto(string Text, TimeSpan Start, TimeSpan End, string Speaker);

public record TranscriptionResult(
    string FullText,
    IReadOnlyList<TranscriptSegmentDto> Segments,
    TimeSpan Duration);

public interface ITranscriptionService
{
    Task<TranscriptionResult> TranscribeAsync(Stream audioStream, string fileName, CancellationToken cancellationToken = default);
}
