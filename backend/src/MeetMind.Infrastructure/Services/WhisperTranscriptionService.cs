using MeetMind.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Audio;

namespace MeetMind.Infrastructure.Services;

public class WhisperTranscriptionOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "whisper-1";
}

public class WhisperTranscriptionService : ITranscriptionService
{
    private readonly OpenAIClient _client;
    private readonly string _model;
    private readonly ILogger<WhisperTranscriptionService> _logger;

    public WhisperTranscriptionService(IOptions<WhisperTranscriptionOptions> options, ILogger<WhisperTranscriptionService> logger)
    {
        _client = new OpenAIClient(options.Value.ApiKey);
        _model = options.Value.Model;
        _logger = logger;
    }

    public async Task<TranscriptionResult> TranscribeAsync(Stream audioStream, string fileName, CancellationToken cancellationToken = default)
    {
        var audioClient = _client.GetAudioClient(_model);

        var options = new AudioTranscriptionOptions
        {
            ResponseFormat = AudioTranscriptionFormat.Verbose,
            TimestampGranularities = AudioTimestampGranularities.Segment | AudioTimestampGranularities.Word
        };

        _logger.LogInformation("Starting Whisper transcription for {FileName}", fileName);

        var response = await audioClient.TranscribeAudioAsync(audioStream, fileName, options, cancellationToken);

        var fullText = response.Value.Text;
        var segments = new List<TranscriptSegmentDto>();

        if (response.Value.Segments is not null)
        {
            foreach (var segment in response.Value.Segments)
            {
                segments.Add(new TranscriptSegmentDto(
                    segment.Text,
                    segment.StartTime,
                    segment.EndTime,
                    "Unknown"));
            }
        }
        else
        {
            segments.Add(new TranscriptSegmentDto(
                fullText,
                TimeSpan.Zero,
                response.Value.Duration ?? TimeSpan.Zero,
                "Unknown"));
        }

        _logger.LogInformation(
            "Whisper transcription completed for {FileName}. Duration: {Duration}",
            fileName,
            response.Value.Duration);

        return new TranscriptionResult(fullText, segments, response.Value.Duration ?? TimeSpan.Zero);
    }
}
