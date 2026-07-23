# Week 1 Decisions

## Backend Foundation
- **Clean Architecture**: Domain, Application, Infrastructure, API layers.
- **.NET 9 Web API** with controllers and MediatR for CQRS-style use cases.
- **PostgreSQL 16 + pgvector** for relational data and future semantic search.
- **ASP.NET Core Identity + JWT** for authentication; roles seeded at startup.

## File Storage
- **Local disk** for development via `LocalDiskMeetingStorageService`.
- Configurable `StorageOptions` to swap in Azure Blob / AWS S3 later.

## Transcription Pipeline
- **OpenAI Whisper** wrapped in `ITranscriptionService`.
- **Polly retry** with exponential backoff (3 attempts, 2s base delay).
- Background hosted service `MeetingProcessingService` processes uploaded meetings.

## Observability
- **Serilog** with console and rolling file sinks (`logs/meetmind-.log`).
- **Health checks** at `/health` with database connectivity check.

## Testing
- Integration tests using `WebApplicationFactory<Program>` and EF Core InMemory.
- Upload flow tests cover valid MP3 upload and invalid file-type rejection.

## Next Week
- GPT-4o summarization and action-item extraction.
- Meeting detail endpoints and transcript retrieval.
- Frontend upload UI and dashboard.
