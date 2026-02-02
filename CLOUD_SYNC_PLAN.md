# EcoRoot — Cloud Sync Plan (Phase 4)

This plan outlines an **optional** cloud sync feature that respects privacy and school contexts.

## Goals
- Allow students to back up progress across devices.
- Keep the default experience **local-only**.
- Require explicit consent (guardian/teacher) for minors.

## Non‑Goals
- Social features or public profiles.
- Sharing personal data without consent.

## Proposed Architecture
- **Client:** .NET MAUI app
- **Backend:** Minimal REST API (Azure Functions or ASP.NET Minimal API)
- **Storage:** Azure Table/Blob or PostgreSQL
- **Identity:** Optional email/OTP or school-issued codes

## Data Model (Sync)
- Student ID (anonymous GUID)
- Completed lessons (moduleId + lessonId)
- Quiz attempts (correct/total + timestamp)
- Last sync time

## Consent & Privacy Flow
1. User taps “Enable Cloud Sync”
2. Consent screen with clear data list + purpose
3. If minor: require parent/guardian confirmation (teacher can provide a classroom code)
4. Store consent timestamp + method
5. Allow opt‑out + data deletion

## Offline & Conflict Strategy
- Default: **last‑write‑wins** for progress and quiz attempts
- Merge strategy: union of completed lessons + append quiz attempts

## Security
- HTTPS only
- Minimal PII (no names by default)
- Token-based authentication
- Data deletion endpoint

## Milestones
- **Phase 4a:** API contract + consent screens
- **Phase 4b:** Sync read/write + conflict merge
- **Phase 4c:** QA + privacy review

---
Last updated: 2026‑02‑02
