# MeetMind AI — 30-Day Development Plan

> **Goal:** Build the MVP for MeetMind AI: a smart meeting notes platform with transcription, AI summaries, action items, semantic search, team collaboration, and Stripe billing.

---

## Overview

| Phase | Dates | Theme | Key Outcome |
|-------|-------|-------|-------------|
| **Week 1** | Days 1–7 | Foundation & Core Pipeline | Project scaffolded; upload → transcript flow working |
| **Week 2** | Days 8–14 | AI Engine & Intelligence | Summary, action items, decisions, topics, search |
| **Week 3** | Days 15–21 | UI/UX, Collaboration & Exports | Polished dashboard, detail views, team features |
| **Week 4** | Days 22–30 | Billing, Limits, DevOps & Launch | Stripe plans, deployment, performance tuning |

**Working cadence:** 1-week sprints, daily 30-min standups (self), end-of-week review and demo.

---

## Week 1: Foundation & Core Pipeline

### Day 1 — Project Scaffold & Tooling
- [ ] Initialize Git repository and branch strategy (`main`, `dev`, feature branches).
- [ ] Create backend solution: .NET 9 Web API with Clean Architecture folders:
  - `MeetMind.Domain`
  - `MeetMind.Application`
  - `MeetMind.Infrastructure`
  - `MeetMind.API`
- [ ] Add global packages: EF Core, ASP.NET Core Identity, JWT, OpenAI SDK, Stripe.NET, pdfsharp/Markdig.
- [ ] Initialize frontend: React 19 + Vite + TypeScript + Tailwind CSS.
- [ ] Add frontend tooling: React Router v7, TanStack Query, Zustand, Axios, shadcn/ui or Headless UI.
- [ ] Add `.editorconfig`, `.gitignore`, `README.md`, and `docker-compose.yml` skeleton.
- [ ] Set up environment variable templates (`.env.example`).

**Deliverable:** Repo builds and runs empty API + Vite dev server.

---

### Day 2 — Database Design & Setup
- [ ] Design core entities:
  - `ApplicationUser`, `Meeting`, `TranscriptSegment`, `Summary`, `ActionItem`, `KeyDecision`, `TopicSegment`, `Comment`, `Team`, `TeamMember`, `Subscription`, `UsageRecord`.
- [ ] Configure PostgreSQL 16 with `pgvector` extension in Docker Compose.
- [ ] Set up EF Core DbContext, migrations, and seed roles/plans.
- [ ] Define vector columns for semantic search (`vector(1536)` or model-appropriate).
- [ ] Add indexes: full-text search (GIN) on transcripts, vector similarity index (ivfflat/hnsw).

**Deliverable:** `docker-compose up` spins up PostgreSQL + pgvector; initial migration applies cleanly.

---

### Day 3 — Authentication & Authorization
- [ ] Implement ASP.NET Core Identity with JWT bearer tokens.
- [ ] Build endpoints: `POST /api/auth/register`, `POST /api/auth/login`, `POST /api/auth/refresh`, `POST /api/auth/me`.
- [ ] Add roles: `Free`, `Pro`, `Team`, `Enterprise`.
- [ ] Add JWT middleware, authorization policies, and `[Authorize]` guards.
- [ ] Frontend: login, register, protected route wrapper, auth store.

**Deliverable:** Users can register/log in; protected routes redirect unauthenticated users.

---

### Day 4 — Frontend Shell & Routing
- [ ] Create app shell: sidebar/navbar, layout, theme, Tailwind config.
- [ ] Set up routes:
  - `/` landing page
  - `/dashboard`
  - `/meetings/:id`
  - `/search`
  - `/team`
  - `/settings`
- [ ] Build reusable components: `Button`, `Card`, `Modal`, `Toast`, `Loader`.
- [ ] Add responsive mobile navigation.

**Deliverable:** Navigation between key screens works on desktop and mobile.

---

### Day 5 — Audio Upload Backend
- [ ] Implement `IMeetingStorageService` for local disk (dev), with Azure Blob / S3 abstraction.
- [ ] Create `POST /api/meetings/upload` endpoint with multipart form handling.
- [ ] Validate file types: MP3, WAV, M4A; enforce max file size per plan.
- [ ] Track upload progress and store file metadata.
- [ ] Save `Meeting` record with status `Uploading` → `Uploaded` → `Processing`.

**Deliverable:** API accepts audio uploads and persists files locally with metadata.

---

### Day 6 — Frontend Upload & Whisper Integration
- [ ] Build drag-drop upload zone in Dashboard with progress indicator.
- [ ] Wire upload to backend API.
- [ ] Integrate OpenAI Whisper API for transcription.
- [ ] Store timestamped transcript segments (`TranscriptSegment` table).
- [ ] Update meeting status via background processing (in-memory queue or Hangfire/Quartz skeleton).

**Deliverable:** Uploading an audio file produces a timestamped transcript.

---

### Day 7 — Week 1 Review & Pipeline Hardening
- [ ] End-to-end test: upload → storage → transcription → database.
- [ ] Add error handling and retry logic for Whisper failures.
- [ ] Add logging (Serilog) and basic API health checks.
- [ ] Write first integration test for upload/transcribe flow.
- [ ] Document Week 1 decisions in `docs/week1.md`.

**Deliverable:** Stable upload-to-transcript pipeline; demo recorded.

---

## Week 2: AI Engine & Intelligence

### Day 8 — Smart Summary Generation
- [ ] Build `ISummaryService` using GPT-4o.
- [ ] Prompt engineering: concise meeting summary, bullet points, tone.
- [ ] Store summary in `Summary` table linked to meeting.
- [ ] Background job triggers summary after transcription completes.
- [ ] Frontend: Summary tab on Meeting Detail page.

**Deliverable:** Meeting summary generated and displayed after transcription.

---

### Day 9 — Action Item Extraction
- [ ] Prompt GPT-4o to extract action items with owner inference and deadline detection.
- [ ] Parse structured output (JSON mode / function calling).
- [ ] Save `ActionItem` records with status: `Todo`, `InProgress`, `Done`.
- [ ] Allow users to edit owner, deadline, status.

**Deliverable:** Action items extracted and editable.

---

### Day 10 — Key Decisions Extraction
- [ ] Prompt GPT-4o to identify key decisions from transcript.
- [ ] Store `KeyDecision` records with context quote.
- [ ] Frontend: Decisions panel on Summary tab.

**Deliverable:** AI-identified decisions displayed per meeting.

---

### Day 11 — Topic Segmentation
- [ ] Segment transcript into topics with timestamps.
- [ ] Store `TopicSegment` records (title, start/end, summary).
- [ ] Frontend: topic sidebar/chapters on transcript view.

**Deliverable:** Meeting broken into navigable topic segments.

---

### Day 12 — Full-Text Search
- [ ] Implement `GET /api/search?q=...` using PostgreSQL full-text search.
- [ ] Search across transcripts, summaries, action items, decisions.
- [ ] Frontend: search page with filters (date, team).

**Deliverable:** Users can keyword-search across all meetings.

---

### Day 13 — Semantic Search with pgvector
- [ ] Generate embeddings for summaries/transcripts using OpenAI embeddings API.
- [ ] Store vectors and implement cosine similarity search.
- [ ] Add toggle in UI: keyword vs. semantic search.

**Deliverable:** Semantic search returns meetings by meaning.

---

### Day 14 — Week 2 Review & Prompt Refinement
- [ ] Evaluate AI output quality on 3–5 sample meetings.
- [ ] Refine prompts for summary, action items, decisions, topics.
- [ ] Add caching for repeated AI calls.
- [ ] Write tests for search endpoints.

**Deliverable:** AI features stable and demoable.

---

## Week 3: UI/UX, Collaboration & Exports

### Day 15 — Dashboard
- [ ] Build dashboard screen:
  - Drag-drop upload zone
  - Recent meetings list
  - Usage bar / plan limits
  - Quick stats (meetings this month, minutes processed)
- [ ] Connect to backend APIs.

**Deliverable:** Functional, responsive dashboard.

---

### Day 16 — Meeting Detail Page
- [ ] Build meeting detail shell with tabs:
  - Summary
  - Transcript
  - Action Items
  - Comments
- [ ] Display meeting metadata, duration, status.

**Deliverable:** Meeting detail page with tab navigation.

---

### Day 17 — Transcript Viewer
- [ ] Scrollable transcript with timestamps.
- [ ] Speaker colors/labels (diarization placeholder or manual assignment).
- [ ] In-transcript search and highlight.
- [ ] Click timestamp to jump to audio position (if audio player added).

**Deliverable:** Searchable, readable transcript viewer.

---

### Day 18 — Action Items Kanban
- [ ] Build Kanban board: To Do → In Progress → Done.
- [ ] Drag-and-drop to update status.
- [ ] Inline edit owner/deadline.
- [ ] Filter by assignee / meeting.

**Deliverable:** Interactive Kanban board for action items.

---

### Day 19 — Search UI Polish
- [ ] Combine full-text and semantic search in one UI.
- [ ] Add filters: date range, team, meeting owner.
- [ ] Display result snippets and relevance scores.

**Deliverable:** Polished search experience.

---

### Day 20 — Export Features
- [ ] Backend: generate PDF summary + transcript.
- [ ] Backend: generate Markdown export.
- [ ] Endpoints: `GET /api/meetings/:id/export/pdf`, `.../markdown`.
- [ ] Frontend: export buttons on meeting detail.

**Deliverable:** PDF and Markdown exports working.

---

### Day 21 — Team Workspace & Comments
- [ ] Implement `Team` and `TeamMember` entities.
- [ ] Share meetings with team members.
- [ ] Add comments on meetings and action items.
- [ ] Team settings page skeleton.

**Deliverable:** Users can share meetings and comment.

---

## Week 4: Billing, Limits, DevOps & Launch

### Day 22 — Stripe Integration
- [ ] Set up Stripe account, products, and prices for Pro/Team plans.
- [ ] Implement `IPaymentService` with Stripe.NET.
- [ ] Endpoints: create checkout session, customer portal, webhook.
- [ ] Update user role on successful subscription.

**Deliverable:** Stripe checkout and customer portal working.

---

### Day 23 — Plan Limits & Enforcement
- [ ] Define limits per tier (meetings/month, max duration, features).
- [ ] Enforce limits at upload time.
- [ ] Track usage in `UsageRecord` table.
- [ ] Show upgrade prompts in UI when limits reached.

**Deliverable:** Billing limits enforced correctly.

---

### Day 24 — Team Settings & Members
- [ ] Complete team settings page.
- [ ] Invite members, manage roles (admin/member).
- [ ] Display billing status per team.

**Deliverable:** Team management functional.

---

### Day 25 — Calendar Integration (Mock)
- [ ] Build mock Google/Outlook calendar sync UI.
- [ ] Add scheduled meeting import placeholder.
- [ ] Store mock calendar connections.

**Deliverable:** Calendar integration UI demo-ready.

---

### Day 26 — Performance Optimization
- [ ] Optimize 30-minute audio processing pipeline.
- [ ] Compress audio before Whisper where possible.
- [ ] Add background job queue (Hangfire/Quartz/Bull if Node worker).
- [ ] Target: 30-min audio → transcript + summary in < 3 minutes.

**Deliverable:** Performance benchmark met.

---

### Day 27 — Testing, Bug Fixes & Security
- [ ] Write unit + integration tests for critical paths.
- [ ] Security review: auth, file validation, SQL injection, XSS.
- [ ] Input sanitization and rate limiting.
- [ ] Fix bugs from manual QA.

**Deliverable:** Test suite passing; security basics covered.

---

### Day 28 — Docker & Docker Compose
- [ ] Containerize API, frontend, and PostgreSQL.
- [ ] Create production-ready `docker-compose.yml`.
- [ ] Add health checks and reverse proxy (nginx/traefik).
- [ ] Test local full-stack deployment.

**Deliverable:** App runs end-to-end via Docker Compose.

---

### Day 29 — Production Deployment
- [ ] Deploy to Railway or Render.
- [ ] Configure environment variables in production.
- [ ] Set up custom domain and SSL.
- [ ] Configure Stripe webhooks for production.

**Deliverable:** Live deployed app on custom domain.

---

### Day 30 — Final Testing, Documentation & Launch Prep
- [ ] Run full smoke tests against production.
- [ ] Verify success criteria checklist.
- [ ] Write user-facing README and basic help docs.
- [ ] Prepare launch assets (landing page copy, screenshots).
- [ ] Plan post-launch monitoring and feedback loop.

**Deliverable:** MVP launched and ready for users.

---

## Success Criteria Tracking

| # | Criterion | Target Completion |
|---|-----------|-------------------|
| 1 | Upload 30-min audio → transcript + summary in < 3 minutes | Day 26 |
| 2 | Action items extracted with assignable owners | Day 9 / Day 18 |
| 3 | Search finds meetings by meaning, not just keywords | Day 13 |
| 4 | Export to PDF/Markdown works | Day 20 |
| 5 | Team can share and comment | Day 21 |
| 6 | Stripe billing enforces limits correctly | Day 23 |
| 7 | Deployed with custom domain | Day 29 |

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Whisper API latency/cost | High | Compress audio, use async jobs, cache results |
| GPT-4o output inconsistency | Medium | Use JSON mode, refine prompts, add fallback parsing |
| Stripe webhooks failing | High | Implement idempotency, logs, retry logic |
| Performance > 3 min for 30 min audio | High | Background queue, chunked processing, file compression |
| pgvector setup issues | Low | Docker Compose with official pgvector image |
| Scope creep | High | Stick to MVP; defer analytics, real-time, native apps |

---

## Suggested Folder Structure

```
MeetMind/
├── backend/
│   ├── MeetMind.API/
│   ├── MeetMind.Application/
│   ├── MeetMind.Domain/
│   ├── MeetMind.Infrastructure/
│   └── tests/
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── hooks/
│   │   ├── stores/
│   │   ├── services/
│   │   └── types/
│   └── tests/
├── ai-prompts/
│   ├── summary.md
│   ├── action-items.md
│   ├── decisions.md
│   └── topics.md
├── docker/
│   ├── Dockerfile.api
│   ├── Dockerfile.web
│   └── nginx.conf
├── docs/
└── docker-compose.yml
```

---

## Notes

- Keep AI prompts under version control in `ai-prompts/`.
- Use feature flags for incomplete integrations (e.g., calendar sync).
- Prioritize end-to-end flow over perfect UI in the first two weeks.
- Schedule buffer time on Days 7, 14, 21, and 27 for bug fixing and polish.
