# EcoRoot

EcoRoot is a .NET MAUI learning app for senior high school students that builds understanding of soil pollution through micro‑lessons, quizzes, and progress tracking.

## Features (MVP)
- Home dashboard with progress summary
- Modules → lessons → quiz flow
- Glossary and progress dashboards
- Offline content seed + local cache
- SQLite progress + quiz attempt tracking

## Project Structure
- `EcoRoot/` — MAUI app source
- `CONTENT_BLUEPRINT.md` — MVP lesson plan
- `UX_FLOW.md` — UX navigation map
- `MVP_READINESS_CHECKLIST.md` — release checklist

## Requirements
- .NET SDK 9+ (or 10 preview used by MAUI)
- MAUI workloads: `android`, `ios`, `maccatalyst`, `windows`

## Build & Run
> Note: MAUI workloads must be installed before restoring packages.

### Install workloads (Windows)
- `dotnet workload install maui-windows maui-android maui-ios maui-maccatalyst`
- `dotnet workload restore`

### Install workloads (macOS)
- `dotnet workload install maui-maccatalyst maui-ios maui-android`
- `dotnet workload restore`

### Verify workloads
- `dotnet workload list`

### Troubleshooting
- If you see `NETSDK1147` (missing workload), run the install command above and retry restore.

### Restore
- `dotnet restore EcoRoot/EcoRoot.csproj`

### Run (Windows)
- `dotnet build EcoRoot/EcoRoot.csproj -t:Run -f net9.0-windows10.0.19041.0`

### Run (Android)
- `dotnet build EcoRoot/EcoRoot.csproj -t:Run -f net9.0-android`

## Content
The packaged content seed lives at:
- `EcoRoot/Resources/Raw/content_seed.json`

The app caches this content locally in:
- App data directory → `content_seed.json`

## Local Storage
- Progress DB: `ecoroot.db3` (SQLite) in app data directory
- Legacy progress file (migrated if present): `progress.json`

## Roadmap
See `DEVELOPMENT_PLAN.md`.

---
Last updated: 2026‑02‑02
