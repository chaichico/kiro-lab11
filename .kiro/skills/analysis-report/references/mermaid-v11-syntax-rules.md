# Mermaid v11 Syntax Rules — Reference

Mermaid v11 is strict about syntax. Follow these rules to avoid "Syntax error in text" failures.

## ER Diagrams (`erDiagram`)
- Valid attribute types: `string`, `int`, `float`, `boolean`, `date`, `datetime`. Do NOT use language-specific types like `guid`, `uniqueidentifier`, `nvarchar`, `decimal`.
- Convert all non-standard types to the closest valid type (e.g., `guid` → `string`, `datetime2` → `datetime`, `decimal` → `float`).
- Relationship labels must be quoted: `CUSTOMERS ||--o{ ORDERS : "has many"` (not `: has many`).

## Sequence Diagrams (`sequenceDiagram`)
- Participant aliases must NOT contain special characters: no parentheses `()`, no slashes `/`, no dots `.`, no brackets `[]`.
- BAD: `participant UI as App.View (JS)` — GOOD: `participant UIView as App View`
- BAD: `participant DB as AppContext/SQL` — GOOD: `participant DB as Database`
- Message text must NOT contain brackets `[]` or special chars. BAD: `Ctrl->>Ctrl: [Auth] check` — GOOD: `Ctrl->>Ctrl: Auth check`
- Message text must NOT contain URL paths with slashes. BAD: `GET /api/items` — GOOD: `GET api items`

## Class Diagrams (`classDiagram`)
- Stereotype markers (`<<interface>>`, `<<enumeration>>`, `<<abstract>>`) are problematic in HTML `<pre>` blocks because `<<` conflicts with the HTML parser. AVOID using stereotypes entirely.
- Instead of `<<enumeration>>`, name the class descriptively (e.g., `class RatingEnum` instead of using a stereotype on `class Rating`).
- Example — instead of:
  ```
  class Rating {
      <<enumeration>>
      One
      Two
  }
  ```
  Write:
  ```
  class RatingEnum {
      One
      Two
      Three
  }
  ```

## State Diagrams (`stateDiagram-v2`)
- Multi-line `note` blocks (`note right of X ... end note`) are fragile in v11. Use single-line notes instead: `note right of Active : Record saved to DB.`
- Transition labels: add a space before the colon: `[*] --> Active : User submits form` (not `[*] --> Active: User submits form`).

## Flowcharts (`flowchart TD/LR`) and Graphs (`graph LR/TD`)
- Node text with special characters (slashes, dots, ampersands, plus signs) MUST be wrapped in double quotes: `A["User navigates to Login page"]` not `A[User navigates to /User/Login]`.
- Decision nodes (rhombus) with special chars also need quotes: `E{"Email provided?"}`.
- Cylinder nodes for databases: use `[("label")]` with quotes inside: `SQL[("SQL Server")]` not `SQL[(SQL Server)]`.
- Edge labels with special chars need quotes: `-->|"EF6"| SQL` or keep simple: `-->|EF6| SQL`.

## General Rules (all diagram types)
- No HTML entities inside `<pre class="mermaid">` blocks — mermaid parses raw text, not HTML.
- No trailing whitespace on lines — can cause parse errors in some diagram types.
- Test mentally: if a label contains any of `/ . ( ) [ ] + & < >`, wrap it in double quotes or rewrite it.
