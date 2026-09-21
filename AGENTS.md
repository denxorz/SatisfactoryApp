# SatisfactoryApp Agent Instructions

Shared guidance for Cursor, GitHub Copilot, and other agents. Cursor-specific workflows and agent roles live in `.cursor/AGENTS.md`.

## Environment

- Development OS: Windows (PowerShell)
- Stack: .NET 10, Blazor WebAssembly (`SatisfactoryApp`), WPF desktop (`SatisfactoryDesktop`)
- Related packages: `Denxorz.Satisfactory.Routes` (satisfactory-routes) and SatisfactorySaveNet; local project references may replace NuGet during integration work
- Typical app run: `dotnet watch run` on the `SatisfactoryApp` project

## Preferences

- Only mention secrets when the task involves committing or sharing sensitive data
- Only add comments when intent is not obvious from the code

## Testing and builds

- Use Debug for day-to-day development
- Use Release for full test suites and save-file parsing (SaveProbe). Release optimizations can surface issues that Debug hides
- After parser or package-reference changes, rebuild dependent projects so PDBs and binaries stay in sync. Stack-trace line numbers can be stale until a clean rebuild

## Local package rewiring

When using local project references instead of NuGet:

- `dotnet sln add` to include the external project
- `dotnet add reference` to convert the NuGet package to a project reference
- Rebuild dependents after changes
- Keep temporary local wiring easy to remove; do not leave long-lived hidden forks

## Save-file parsing

Parsers (`TrainStationParser`, `DroneStationParser`, `TruckStationParser`, `FactoryParser`, and related lookup code) routinely see orphaned or missing identifiers. Defensive lookups are required, not optional.

A dictionary error with key `"??"` usually means **missing or empty identifiers**, not a true duplicate. `GetObjectPathName()` (or similar) returned null/empty and the fallback `"??"` was used more than once.

When mapping save identifiers:

1. Key dictionaries by object path (for example `GetObjectPathName("mStation")`)
2. Drop null/empty keys: `.Where(g => !string.IsNullOrEmpty(g.Key))`
3. Handle duplicates: `GroupBy(...).ToDictionary(g => g.Key, g => g.First())` (or `DistinctBy` when that is clearer)
4. Look up with `TryGetValue` / `ContainsKey`; never index a dictionary that might lack the key
5. After `Select` that can return null, filter: `.Where(s => s is not null)`
6. Log skipped items with `Trace.WriteLine` so missing content can be diagnosed

Also: check collection size before `ToString()` on lists; use `obj?.Property ?? defaultValue` on orphaned references.

Zero stations in a save is valid. Example: Sector 39 Extraction autosaves can have 0 stations and hundreds of factories. Other saves have dozens of stations. Always test parsers against more than one file, including partial/orphaned data.

### Parser trace

Use `System.Diagnostics.Trace.WriteLine` in parsers (works in Debug and Release). SaveProbe should listen on stderr:

```csharp
var consoleListener = new TextWriterTraceListener(Console.Error) { Name = "DebugConsole" };
Trace.Listeners.Add(consoleListener);
```

Existing skip messages:

- `[TrainStationParser] Skipping train station without identifier: {pathName}`
- `[DroneStationParser] Skipping drone station without identifier: {pathName}`
- `[TruckStationParser] Skipping truck station without identifier: {pathName}`
- `[FactoryParser] Skipping miner resource lookup - resource not found: {resourcePath}`

Filter them with:

```powershell
SaveProbe.exe "path.sav" 2>&1 | Select-String "\[.*Parser\]"
```
