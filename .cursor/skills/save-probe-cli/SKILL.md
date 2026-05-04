---
name: save-probe-cli
description: Run the SaveProbe command line parser against Satisfactory .sav files, capture parser failures with full exception chains, and optionally export parsed JSON. Use when a save file cannot be parsed, when debugging Denxorz.Satisfactory.Routes parsing behavior, or when validating parser changes quickly from the terminal.
disable-model-invocation: true
---

# SaveProbe CLI

## When to use

Use this skill when:
- A user says a `.sav` file fails to load or parse.
- You need the exact parser exception from `SaveDetails.LoadFromStream`.
- You want a quick pass/fail parse check without running the UI.

## Command

Run from the repository root:

```bash
dotnet run --project scripts/SaveProbe -- "<path-to-save.sav>"
```

Optional flags:
- `--json`: print parsed JSON to stdout.
- `--json-out <path>`: write parsed JSON to a file.

Example:

```bash
dotnet run --project scripts/SaveProbe -- "Sector 39 Extraction_autosave_0.sav" --json-out "out/save.json"
```

## Expected behavior

- Exit code `0`: parse succeeded; prints entity counts.
- Exit code `1`: usage/argument/file errors.
- Exit code `2`: parser failed; prints exception + inner exceptions + stack trace.

## Workflow

1. Reproduce failure with the exact save file path.
2. Capture and summarize the first parser exception and innermost exception message.
3. If parsing succeeds, use the counts output as a quick sanity check.
4. If needed, export JSON and inspect downstream data consumers.
