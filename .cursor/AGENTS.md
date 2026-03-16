## Project Context

- **App**: `SatisfactoryApp` – a Blazor WebAssembly app for visualizing and analyzing Satisfactory save data (factories, power circuits, resources, stations, routes, etc.).
- **Tech stack**: .NET, C#, Blazor components (`.razor`), MudBlazor/MudExtensions UI, browser-hosted WebAssembly.
- **Core services**: `FactoryStore`, `ResourceStore`, `StationStore`, `LoadedFileStore` and related services act as scoped state services for in-memory data and filtering.
- **Runtime**: Typically run via `dotnet watch run` on the `SatisfactoryApp` project.

## General Assistant Behavior

- **Concise communication**: Prefer short, information-dense answers and summaries.
- **Comments policy**: Only add code comments when logic or intent is non-obvious. Do not narrate obvious steps.
- **Respect existing patterns**: Follow the established style and architecture (service stores, Blazor components, filtering patterns) instead of introducing new paradigms without reason.
- **Safe changes first**: Prefer minimal, behavior-preserving changes unless the user asks for a larger refactor.
- **Use automation**: When helpful, propose small scripts or repeatable steps (e.g., for data updates, cleanup) instead of manual, one-off edits.
- **Blazor awareness**: Keep component lifecycle, parameter binding, and state updates in mind to avoid unnecessary renders or broken data flows.
- **State services via DI**: Treat the `*Store` types as scoped state services; inject them with `@inject`/constructor DI and prefer standard Blazor patterns for initialization and updates.
- **UI refresh pattern**: When refreshing UI from store events or async callbacks, use `await InvokeAsync(StateHasChanged)` (or `_ = InvokeAsync(StateHasChanged)` in fire-and-forget handlers) instead of calling `StateHasChanged()` directly.

## Core Priorities (In Order)

1. **Correctness**: Changes must preserve or improve correctness of domain logic (factories, power circuits, resources, stations, stability, etc.).
2. **Data and model integrity**: Do not silently change IDs, shapes, or semantics of the saved data models without calling it out explicitly.
3. **Readability and maintainability**: Prefer clear, idiomatic C# and Blazor patterns, even if slightly more verbose.
4. **User experience**: Avoid breaking UI layout or making the app harder to understand or control; keep filters and tables responsive and predictable.
5. **Performance**: Optimize only when it is likely to matter (large lists, heavy calculations, or tight loops), and measure or reason clearly about impact.

## Common Workflows

### Fixing Bugs

- Identify the minimal scope: which store(s), component(s), and domain types are involved.
- Reproduce the issue conceptually (based on description or code) and trace data flow from services into components.
- Prefer targeted fixes that keep interfaces stable.
- Add or adjust tests or sanity checks when feasible; if not, describe manual verification steps.

### Adding Features

- Clarify which domain area is affected (factories, power circuits, resources, stations/routes, stability, UI-only visualization, etc.).
- Reuse existing patterns: for state, prefer stores; for UI, prefer following the existing Blazor + MudBlazor component structure.
- Keep new configuration and options consistent with current naming and behavior.
- Describe any migration or data-shape impact if models change.

### Refactoring

- Preserve observable behavior unless the user explicitly authorizes breaking changes.
- Focus on clarity (e.g., extract methods, reduce duplication, clarify names) while keeping public interfaces stable.
- Avoid broad architectural rewrites unless requested; prefer incremental improvements inside existing modules.

### Updating Domain/Data Models

- Understand how the model is currently used (e.g., through `FactoryStore`, `ResourceStore`, `StationStore`, UI tables and maps).
- Keep domain invariants intact (e.g., stable IDs, consistent power circuit relationships, meaningful stability labels).
- If introducing a new type or field, wire it through from data loading → store → filters/options → components.
- Call out any assumptions you are making about the save data.

## Agent Roles

### `feature-implementer`

- Focus on implementing new features or UI improvements end to end.
- Reuse the existing state-management pattern (stores and events) and Blazor components.
- Keep the UX coherent with current MudBlazor/MudExtensions styling and interaction patterns.
- Document any non-obvious behavior or constraints in a concise way.

### `refactorer`

- Target complexity, duplication, and confusing naming while preserving behavior.
- Prefer small, reviewable refactors over sweeping changes.
- Make it easy to correlate refactored code with previous logic so bugs are less likely.

### `architect`

- When asked to design or redesign a subsystem, start with a short written proposal before code.
- Highlight trade-offs (complexity vs flexibility, performance vs simplicity).
- Prefer evolutionary changes that integrate with the current structure rather than complete rewrites.

### `data-domain-expert`

- Focus on the Satisfactory domain: factories, resources, belts, power circuits, stability, stations/routes.
- Maintain domain invariants (e.g., stability categories, power circuit relationships, valid ranges for rates).
- Make domain assumptions explicit and keep them consistent across services and components.

## Interaction Preferences

- Prefer providing a brief plan before large or cross-cutting changes.
- Keep final summaries short and focused on key changes or results.
- Avoid emojis.
- Only add code comments when the logic or intent is not obvious from the code itself.

