---
name: routes-package-integration
description: Handles changes spanning SatisfactoryApp and Denxorz.Satisfactory.Routes, including deciding package-vs-app ownership, local integration testing, and safe package version bumps. Use when route parsing or station/route behaviors may require upstream package updates.
disable-model-invocation: true
---

# Routes Package Integration

## Purpose

Use this skill when work touches both this app and the `Denxorz.Satisfactory.Routes` package.

## Workflow

1. Confirm where the defect or feature belongs:
   - App UI/state wiring (`SatisfactoryApp`)?
   - Route-domain parsing/model behavior (`Denxorz.Satisfactory.Routes`)?
   - Both?
2. If package changes are needed, prefer upstream fixes over app-level duplication.
3. Keep app changes minimal while waiting for package updates:
   - Add narrow guards only when needed.
   - Avoid copying package internals into app code.
4. For package version changes in app:
   - Update the package reference explicitly.
   - Verify routes/stations-related screens and summaries.
   - Call out expected behavior differences in the final summary.

## Local Integration Testing

When fast iteration is needed, use one local strategy:

- Local NuGet source with prerelease package versions.
- Local repository wiring (submodule/subtree/project-reference style workflow) when coordinated changes are required.

Keep temporary local wiring easy to remove and avoid leaving long-lived hidden dependencies.

## Guardrails

- Do not create silent forks of `Denxorz.Satisfactory.Routes`.
- Do not ship accidental temporary test wiring.
- Keep ownership boundaries clear in code and explanations.
