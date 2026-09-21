---
name: save-probe-cli
description: Run the SaveProbe command line parser against Satisfactory .sav files, capture parser failures with full exception chains, optionally export parsed JSON, and optionally scan vehicle/path-related objects. Use when a save file cannot be parsed, when debugging Denxorz.Satisfactory.Routes parsing behavior, or when validating parser changes quickly from the terminal.
disable-model-invocation: true
---

# SaveProbe CLI

## When to use

Use this skill when:
- A user says a `.sav` file fails to load or parse.
- You need the exact parser exception from `SaveDetails.LoadFromStream`.
- You want a quick pass/fail parse check without running the UI.

## Command

Run from the repository root. Use **Release** for save-parse accuracy (Debug can hide issues):

```bash
dotnet run --configuration Release --project scripts/SaveProbe -- "<path-to-save.sav>"
```

Optional flags:
- `--json`: print parsed JSON to stdout.
- `--json-out <path>`: write parsed JSON to a file.
- `--scan-vehicle-paths`: print likely vehicle/path-related object classes and property names.

Example:

```bash
dotnet run --configuration Release --project scripts/SaveProbe -- "Sector 39 Extraction_autosave_0.sav" --json-out "out/save.json"
```

Vehicle/path review example:

```bash
dotnet run --configuration Release --project scripts/SaveProbe -- "Sector 39 Extraction_autosave_0.sav" --scan-vehicle-paths
```

## Expected behavior

- Exit code `0`: parse succeeded; prints entity counts.
- Exit code `1`: usage/argument/file errors.
- Exit code `2`: parser failed; prints exception + inner exceptions + stack trace.

## Workflow

1. Reproduce failure with the exact save file path.
2. Capture and summarize the first parser exception and innermost exception message.
3. If parsing succeeds, use the counts output as a quick sanity check.
4. If you are investigating route/vehicle changes, run `--scan-vehicle-paths` and capture the top matching `TypePath` groups.
5. If needed, export JSON and inspect downstream data consumers.
