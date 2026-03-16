---
name: satisfactory-domain
description: Captures how SatisfactoryApp models Satisfactory factories, power circuits, resources, and stations, and how to safely evolve this domain. Use when changing domain logic, filters, or UI that reflects game mechanics.
---

# Satisfactory Domain for SatisfactoryApp

## When to Use This Skill

Use this skill whenever:

- Working with **factories**, **power circuits**, **resources**, **stations/routes**, or related filters and summaries.
- Changing logic in domain-related services and stores (e.g., `FactoryStore`, `ResourceStore`, `StationStore`).
- Updating UI components that visualize domain data (maps, tables, graphs, stability indicators, power circuit views).
- Adding new domain concepts or metrics that extend the existing Satisfactory modeling.

## Domain Overview

- **Factories**
  - Represent production facilities in the Satisfactory world.
  - Have a `Type` (e.g., assembler, refinery), one or more **power circuit** relationships, and a **production stability** status.
  - Are managed centrally via `FactoryStore`, which holds:
    - All factories and power circuits.
    - Filter state (selected types, power circuits, stability categories).
    - Filtered views used by UI components (tables, maps, summary views).

- **Power Circuits**
  - Model electrical networks connecting factories.
  - Include a notion of **main** vs **sub** circuits:
    - Main circuit keys: `"main_{Id}"`.
    - Sub circuit keys: `"sub_{Id}"`.
  - May have `ParentCircuitId` for hierarchical circuits.
  - Used to build filter options and labels (e.g., via `CircuitNames.GetFilterName`).

- **Stability**
  - Indicates how consistently a factory is producing.
  - Derived from `PercentageProducing` (0–100% or null) and mapped into categories such as:
    - `"Stable"`, `"Almost Stable"`, `"Unstable"`, `"Off"`, `"Unknown"`.
  - These labels are surfaced in UI filters and summaries; they should remain consistent across the app.

- **Resources and Stations**
  - `ResourceStore` and `StationStore` (and related components) manage:
    - Resource nodes, flow summaries, and station graphs/tables.
    - Cargo routes and station-level statistics for trains or transport.
  - UI components build visualizations (tables, graphs, maps) on top of these stores.

## Invariants & Constraints

When editing domain logic, try to preserve these invariants unless explicitly changing behavior:

- **Factory and Power Circuit Relationships**
  - Each factory’s main and sub power circuit IDs should refer to valid circuits present in the power circuit list.
  - The `"main_{id}"` / `"sub_{id}"` key format must stay in sync between filters, options, and filter evaluation.

- **Stability Categories**
  - The mapping from `PercentageProducing` to stability labels must remain consistent wherever used.
  - If changing thresholds or labels, update:
    - Filter predicates.
    - Stability options and counts.
    - Any UI that directly interprets stability values.

- **Filter Behavior**
  - Filters should be **inclusive** and predictable:
    - Empty selections mean “no filter” (include all values for that dimension).
    - Non-empty selections should only exclude based on the selected dimension, not alter unrelated behavior.
  - Updating filter selections should:
    - Refresh the filtered collections.
    - Raise corresponding change events so UI updates correctly.

- **ID and Key Stability**
  - Avoid changing IDs or key formats that are used across stores, filters, and components unless clearly justified.
  - If a change is necessary, ensure all relevant code paths (stores, UI, serialization/parsing) are updated together.

## Typical Changes and How to Approach Them

### Adding a New Factory Type

- Identify how factory types are currently:
  - Stored in domain models.
  - Counted and surfaced in `FactoryTypeOption` lists.
  - Used in filters and UI components.
- Ensure the new type:
  - Is included wherever factory types are enumerated or counted.
  - Appears correctly in filter options (with counts) and in any summaries.

### Adjusting Stability Logic

- If changing thresholds or names (e.g., what counts as “Almost Stable”):
  - Update the stability calculation function (e.g., the mapping from `PercentageProducing` to labels).
  - Update any option lists and labels that depend on stability categories.
  - Consider how this affects existing filters and visual cues in the UI.

### Extending Power Circuit Metrics or Filters

- When adding new power circuit-related views or filters:
  - Reuse the existing pattern of:
    - Computing counts per circuit.
    - Building labeled options for main and sub circuits.
    - Using stable keys for selection and comparison.
  - Keep parent/child circuit relationships clear and consistent with current behavior.

### Adding Resource or Station Features

- Identify the relevant store (`ResourceStore`, `StationStore`) and existing components (tables/maps/graphs).
- Extend models and stores first, then propagate the changes:
  - Data loading/parsing → store fields and collections → filters/options → components.
- Ensure new metrics integrate naturally into existing views (e.g., additional columns, filters, or summary cards).

## Testing & Validation Guidelines

When domain logic changes, prefer to:

- **Sanity-check filters and counts**:
  - Verify that counts in filter options match what you see in tables/maps.
  - Confirm that selecting/deselecting filters includes/excludes the expected factories, resources, or stations.

- **Check stability and power circuits**:
  - Confirm that stability categories are assigned as expected for representative percentages.
  - Check that factories assigned to a given main or sub power circuit appear under the right filters and visuals.

- **Walk through key UI flows**:
  - Load representative saves and step through:
    - Factories tab (maps, tables, filters).
    - Resources views.
    - Stations and cargo flow views.
  - Ensure there are no obvious regressions (missing data, empty tables, broken filters).

If you must make assumptions about save data or game mechanics, state them explicitly and keep them consistent across stores and components.

