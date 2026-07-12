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

### Database

Start PostgreSQL + pgvector:

```bash
docker-compose up -d
```

Apply EF Core migrations:

```bash
cd backend
dotnet tool restore
dotnet ef database update --project src/MeetMind.Infrastructure --startup-project src/MeetMind.API
```

Create a new migration after model changes:

```bash
dotnet ef migrations add <Name> --project src/MeetMind.Infrastructure --startup-project src/MeetMind.API --output-dir Data/Migrations
```

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
