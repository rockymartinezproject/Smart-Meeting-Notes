# MeetMind AI

> Smart Meeting Notes — Transcribe, Summarize, Extract Action Items

Upload a meeting recording. Get transcripts, summaries, and action items in minutes.

## Tech Stack

| Layer      | Technology                                    |
|------------|-----------------------------------------------|
| Backend    | .NET 9 Web API, Clean Architecture            |
| Frontend   | React 19 + Vite + Tailwind CSS                |
| Database   | PostgreSQL 16 + pgvector extension            |
| AI         | OpenAI Whisper + GPT-4o                       |
| Auth       | ASP.NET Core Identity + JWT                   |
| File Storage | Local disk (dev) → Azure Blob / AWS S3 (prod) |
| Payments   | Stripe                                        |
| Deployment | Docker + Docker Compose (dev)                 |

## Project Structure

```
MeetMind/
├── backend/
│   ├── src/
│   │   ├── MeetMind.Domain/
│   │   ├── MeetMind.Application/
│   │   ├── MeetMind.Infrastructure/
│   │   └── MeetMind.API/
│   └── MeetMind.sln
├── frontend/
│   └── src/
├── docker-compose.yml
└── README.md
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js 20+
- PostgreSQL 16 with pgvector (or Docker)

### Backend

```bash
cd backend
dotnet build
dotnet run --project src/MeetMind.API
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

### Docker (full stack)

```bash
docker-compose up -d
```

## Environment Variables

Copy `.env.example` to `.env` and fill in your secrets.

## License

MIT
