# Agent Notes — MeetMind AI

## Local .NET SDK

A .NET 9 SDK is installed locally at `./.dotnet/` so agents don't need a system-wide SDK.
Run dotnet commands like this from the repository root:

```bash
export PATH="$PWD/.dotnet:$PATH"
cd backend
dotnet build
dotnet run --project src/MeetMind.API
```

## Project Layout

- `backend/` — .NET 9 Web API with Clean Architecture
  - `MeetMind.Domain` — entities, enums, interfaces
  - `MeetMind.Application` — use cases, MediatR, validators, mappings
  - `MeetMind.Infrastructure` — EF Core, Identity, external services
  - `MeetMind.API` — controllers, middleware, DI wiring
- `frontend/` — React 19 + Vite + Tailwind CSS
- `docker-compose.yml` — PostgreSQL 16 + pgvector for local dev

## Conventions

- Use minimal changes; follow existing code style.
- Keep domain logic in `MeetMind.Domain` / `MeetMind.Application`.
- Add new packages with explicit versions compatible with `net9.0`.
- Frontend uses Tailwind CSS utility classes and Zustand for state.
