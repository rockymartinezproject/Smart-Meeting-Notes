using MeetMind.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Audio;
using Polly;

namespace MeetMind.Infrastructure.Services;

public class WhisperTranscriptionOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "whisper-1";
    public int RetryCount { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 2;
}

public class WhisperTranscriptionService : ITranscriptionService
{
    private readonly OpenAIClient _client;
    private readonly string _model;
    private readonly int _retryCount;
    private readonly int _retryDelaySeconds;
    private readonly ILogger<WhisperTranscriptionService> _logger;

    public WhisperTranscriptionService(IOptions<WhisperTranscriptionOptions> options, ILogger<WhisperTranscriptionService> logger)
    {
        _client = new OpenAIClient(options.Value.ApiKey);
        _model = options.Value.Model;
        _retryCount = options.Value.RetryCount;
        _retryDelaySeconds = options.Value.RetryDelaySeconds;
        _logger = logger;
    }

    public async Task<TranscriptionResult> TranscribeAsync(Stream audioStream, string fileName, CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _retryCount,
                retryAttempt => TimeSpan.FromSeconds(_retryDelaySeconds * Math.Pow(2, retryAttempt - 1)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Whisper transcription failed for {FileName} (attempt {Attempt}). Retrying in {Delay}...",
                        fileName,
                        retryCount,
                        timeSpan);
                });

        return await retryPolicy.ExecuteAsync(async ct => await TranscribeInternalAsync(audioStream, fileName, ct), cancellationToken);
    }

    private async Task<TranscriptionResult> TranscribeInternalAsync(Stream audioStream, string fileName, CancellationToken cancellationToken)
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
