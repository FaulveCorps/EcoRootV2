# EcoRoot — Localization Notes

## Current Localization Scope
- UI strings are localized via `Resources/Strings.resx` and `Resources/Strings.es.resx`.
- Labels, buttons, hints, and progress/badge text are now mapped to resources.
- Glossary terms are localized using resource keys (see `GlossaryTerm*`).
- Scenario Lab activity copy is localized in resources (see `Activity*`).

## Content Localization Strategy
- Lesson content is currently packaged in `Resources/Raw/content_seed.json` (English only).
- Language-specific content files are supported (e.g., `content_seed.es.json`). `JsonContentService` loads based on `CultureInfo.CurrentUICulture`.
- Keep module/lesson IDs stable across languages to preserve progress tracking and quiz stats.

## Remaining Work (Phase 4)
- Add localized lesson content JSON files for new languages.
- Translate teacher resource item text (if not covered in resources).
- Audit any remaining hard-coded strings in models/services.

---
Last updated: 2026-02-02