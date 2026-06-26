---
name: analysis-report
description: AWS Transform analysis agent — runs ATX comprehensive-codebase-analysis then generates self-contained HTML5 reports
---

# Assessment Report — Content & Quality Rules

Rules for the HTML assessment report generated from ATX Documentation output. The agent prompt defines the workflow (phases, paths, commands). This skill defines what goes in the report and how it should look.

## Self-Contained HTML Rules

- ALL CSS inline in `<style>` tags — read from `templates/atx-analysis-report/assessment-report.css` at generation time, do NOT hardcode from memory
- ALL JavaScript inline in `<script>` tags EXCEPT mermaid.min.js (see Mermaid section)
- NO external dependencies (no CDN links)
- Works on `file://` protocol — NEVER use `fetch()`
- Relative hrefs only for cross-page links and local script references
- Responsive and print-friendly

## Tab Order (Enforced)

1. 📋 Overview (always first, always `active`)
2. 🏗️ Architecture
3. ⚙️ Behavior
4. 📊 Analysis
5. ⚠️ Technical Debt
6. 📈 Diagrams
7. 📚 Reference
8. 🔧 Specialized
9. 🚀 Migration
10. 💡 Recommendations & Roadmap (always last)

Unknown/extra areas from the manifest: place between Specialized (8) and Migration (9).

## Tab Content — Minimum Requirements

Every tab MUST include:
- Metric cards (`.stats-grid` > `.stat-card`) with real numbers — see Per-Tab Required Content for minimum counts per tab
- 2+ tables summarizing key findings
- Contextual alerts/callouts for critical findings
- "📄 View full details →" link to the detail page (except the Recommendations & Roadmap tab, which is AI-generated and has no detail page)

Tabs should give 80% of the value without clicking detail pages.

### Thin Tab Prevention
If a tab area has only 1 input file, still produce a thorough summary. Do not produce a tab with 1-2 sentences. If genuinely minimal, merge with a related tab.

## Per-Tab Required Content

| Tab | Min Cards | Required Content |
|-----|-----------|-----------------|
| Overview | 6+ | **Executive Dashboard** heading. Plain-language "What is this system?" summary (2-3 sentences, zero jargon). Migration Readiness Score (1-10 with justification), key risks with business-impact language, technology stack table, business domain table |
| Architecture | 4+ | System diagram (layered/flow), design patterns table with badges, component inventory, data source connections, anti-patterns with severity |
| Behavior | 4+ | Workflow flow diagram AND phase table, domain-specific logic, scheduled jobs or triggers (if applicable), error handling patterns, critical path analysis |
| Analysis | 4+ | Complexity distribution table, security alerts for EVERY finding, dependencies grouped by category, code metrics cards, **Code Metrics Comparison Table** (compare against industry thresholds) |
| Technical Debt | 4+ | Severity-ranked inventory with quantified impact, top duplicated items, risk matrix (likelihood × impact), remediation effort estimates |
| Diagrams | 4+ | Recreate ALL key diagrams from the source markdown. Use `<pre class="mermaid">` blocks for ER diagrams, sequence diagrams, class diagrams, state diagrams, and flowcharts — copy mermaid syntax from source markdown files. Use CSS components (`.flow-diagram`, `.diagram-layers`) for simple linear flows and layered architecture views. ASCII art diagrams from the source should be placed in `<pre>` blocks. Each section needs a heading. Include: entity relationships, request flows, component dependencies, state machines, data flow |
| Reference | 6+ | Data model summary, schema organization, entity relationships, parameter inventory, interface/API patterns, **Interface Consumption table** (if applicable) |
| Specialized | 5+ | Title as "Specialized — {{DOMAIN_CONTEXT}}". Summarize ALL input files. Business rules with conditional logic in collapsible sections |
| Migration | 6+ | Phase table with job counts/priorities, Go/No-Go Checkpoints (business-outcome language), dependency chain, test specs, risk items |
| Recommendations & Roadmap | 4+ (Investment Summary) | **4-layer AWS target architecture diagram** (Ingress → Application → Data → Security). Component mapping table (current → AWS target). Prioritized remediation in 3 tiers (immediate / during migration / post-migration). Migration risk table. **Investment Summary** (REQUIRED): 4 metric cards (Current Annual Cost, Post-Migration Annual Cost, Migration Investment, Estimated Payback Period). **Cost of Inaction** alert with 3-5 business risks. Recommended approach comparison table, phased migration plan with durations, timeline flow diagram, **Estimated Migration Investment** (team size, duration, total cost). **AWS Transform Custom + Kiro** feasibility assessment. Next steps. Always LAST tab |

## Board-Readability

Reports are shared with non-technical board members and customers.

- Overview "What is this system?" must use zero technical jargon
- Key Risks must include business consequences, not just technical descriptions
- Migration checkpoints must describe business outcomes, not technical gates
- Use ranges for estimates ("9–14 months", "$400K–$600K"), never false precision
- Do NOT include real-time AWS service pricing tables — use AI-generated estimates only
- Include a business-impact line in the critical banner when possible

### Financial Framing (AI-Generated Estimates)
- Investment Summary: 4 metric cards with ranges (Current Annual Cost, Post-Migration Annual Cost, Migration Investment, Estimated Payback Period)
- Current Annual Cost: break down into components (licensing, developer premium, infrastructure, risk)
- Payback Period: calculate as Migration Investment ÷ Annual Savings
- Cost of Inaction: structured bullet list with 3-5 business risks
- Estimated Migration Investment in Roadmap: team size, duration, total cost

## Data Consistency

Numbers MUST be traceable to input data. Do not invent or reinterpret counts.

- Critical banner: count ONLY CRITICAL and HIGH severity items
- Security stat card: total distinct vulnerability count, broken down by severity
- Security alerts in Analysis tab: one alert per finding, all severity levels (`alert-danger` for critical/high, `alert-warning` for medium, `alert-info` for low, `alert-success` for implemented/mitigated)
- LOC, file counts, table counts: exact numbers from source
- Cross-tab consistency: same number everywhere for the same metric
- Technical Debt inventory: the table row count MUST equal the total shown in stat cards (e.g., if cards say 6 High + 11 Medium + 6 Low = 23 total, the table MUST have exactly 23 rows)

### Data Deduplication Rules

Each tab has a distinct PURPOSE. Do not repeat the same table or the same metric cards across tabs. Follow these ownership rules:

| Data | Owner Tab | Other tabs may... |
|------|-----------|-------------------|
| Severity counts (High/Medium/Low) | Technical Debt | Overview may show per-severity stat cards (executive dashboard needs the breakdown); other tabs may reference the total in a single stat card. Do NOT repeat the full inventory table outside Technical Debt. |
| Security findings (full list) | Analysis | Overview may list top 3 risks as alerts; Technical Debt may reference count only |
| LOC / file counts | Overview | Architecture may show per-layer breakdown (different cut of same data) |
| Dependency list | Analysis | Architecture may show dependency *graph/diagram* (visual, not table repeat) |
| API endpoints | Reference | Behavior may show workflow *flow diagrams* that reference endpoints |
| Database schema | Reference or Specialized | Diagrams may show entity *relationship diagram* (visual, not table repeat) |
| Migration phases | Migration | Roadmap shows *timeline + cost*, not the same phase table again |
| Test specifications | Migration | Do not repeat in other tabs |

**Rule of thumb**: If the same HTML table appears in two tabs, one of them is wrong. Use a different *representation* (diagram vs table, summary stat vs full list, flow chart vs inventory).

### Root-Level File Routing

ATXDocumentation may contain root-level markdown files (e.g., `README.md`, `project-overview.md`, `technical-debt-report.md`) alongside area subfolders. The Python detail page generator groups all root-level files into the "overview" detail page. When building index.html tabs, route content from these files to the correct tab based on their subject matter:

| Root File | Route to Tab |
|-----------|-------------|
| `README.md` | Overview (navigation index — use for cross-references, not content) |
| `project-overview.md` | Overview (tech stack, codebase size, key features) |
| `technical-debt-report.md` | Technical Debt (executive summary, severity tables) |
| Other root `.md` files | Read content and route to the most relevant tab |

Do NOT dump all root-level file content into the Overview tab. Read each file and extract content for the appropriate tab.

**Important:** The Python detail page generator groups all root-level files into the "overview" detail page by directory structure. This does NOT determine which index.html tab gets the content. When building index.html tabs, you must read root-level files and extract content for the correct tab based on subject matter, regardless of which detail page they appear in.

### Migration Readiness Score Rubric

| Score | Criteria |
|-------|----------|
| 1–2 | No migration path, critical security vulns, platform unsupported, zero tests, no docs |
| 3–4 | Full rewrite required, critical security, platform EOL, some structure exists |
| 5–6 | Incremental migration possible, moderate security, some reusable components |
| 7–8 | Clear migration path, minor security, most components reusable, good tests |
| 9–10 | Near-ready, minimal issues, comprehensive tests, well-documented |

## Technical Depth (Do Not Drop)

- Analysis: Code Metrics Comparison Table + ALL security findings
- Behavior: Detailed mapping/processing flow diagrams
- Specialized: Collapsible business rules
- Reference: Interface consumption patterns
- Technical Debt: Remediation effort estimates per item

## AWS Transform Custom + Kiro Focus

The Recommendations & Roadmap tab should frame the migration approach around **AWS Transform Custom analysis** (this report) combined with **Kiro** as the AI-assisted development tool for executing the migration.

- **AWS Transform Custom**: Completed the comprehensive codebase analysis (this report). Reference the ATX conversation ID and agent minutes from the execution metadata.
- **Kiro**: Recommended as the development environment for executing the migration. Kiro's agent-assisted coding, specs, and hooks can accelerate each migration phase.
- **Feasibility table**: Include a row for "ATX Comprehensive Analysis" (Completed ✓), a row for any ATX-managed transformation (Available / Not Available), and a row for "Kiro-assisted manual migration" (Recommended ✓).
- Do NOT recommend generic tools like "Microsoft Upgrade Assistant" as the primary approach — position Kiro + ATX as the primary workflow.

## Accessibility

- Include `<a href="#main-content" class="skip-link">Skip to main content</a>` as the first element in `<body>`
- Add `id="main-content"` on the `.container` div
- No dark mode / theme toggle — light theme only

## Mermaid.js Diagrams (index.html only)

The index.html page uses mermaid.js for rendering proper diagrams. The agent MUST:

1. Copy `templates/atx-analysis-report/mermaid.min.js` to `atx-analysis-report/<project>/mermaid.min.js` (done alongside the logo copy)
2. Reference it via `<script src="mermaid.min.js"></script>` before the closing `</body>` — do NOT inline the 2.6MB file into the HTML
3. Add `<script>mermaid.initialize({startOnLoad:false,theme:'default',securityLevel:'loose'});</script>` after the mermaid.js script tag
4. Use `<pre class="mermaid">` blocks for diagrams that benefit from proper rendering:
   - **ER diagrams** (`erDiagram`) — entity relationships with cardinality
   - **Sequence diagrams** (`sequenceDiagram`) — request flows with lifelines
   - **Class diagrams** (`classDiagram`) — data models with properties and relationships
   - **State diagrams** (`stateDiagram-v2`) — lifecycle state machines
   - **Flowcharts** (`flowchart TD/LR`) — complex decision trees, component dependencies
5. Keep CSS components (`.flow-diagram`, `.diagram-layers`, `.aws-diagram`) for:
   - Simple linear flows (timeline, migration phases, service map)
   - AWS target architecture (4-layer diagram)
   - Layered architecture views
6. Copy mermaid syntax from the source markdown where it exists — adapt as needed for the report context
7. Detail pages do NOT get mermaid.js — they show mermaid code blocks with the `.mermaid-fallback` label

### Mermaid Block Format
```html
<pre class="mermaid">
erDiagram
    CUSTOMERS ||--o{ ORDERS : "places"
    PRODUCTS ||--o{ ORDERS : "contains"
</pre>
```

Do NOT wrap in `<div class="mermaid">` — use `<pre class="mermaid">` for proper whitespace handling.

### Mermaid v11 Syntax Rules (CRITICAL — prevents render errors)

Read `.kiro/skills/analysis-report/references/mermaid-v11-syntax-rules.md` for the full reference. Key points:
- ER diagrams: only use `string`, `int`, `float`, `boolean`, `date`, `datetime` as attribute types. Quote relationship labels.
- Sequence diagrams: no special characters in participant aliases or message text.
- Class diagrams: avoid `<<stereotype>>` markers entirely — they conflict with HTML `<pre>` parsing.
- State diagrams: use single-line notes only.
- Flowcharts: wrap node text containing special characters in double quotes.
- General: no HTML entities inside mermaid blocks, no trailing whitespace.

## Critical Banner (Conditional)

- Render `<div class="critical-banner">` if there are critical OR high-severity issues (count > 0)
- The banner should list the specific critical/high issues as a bullet-separated summary
- Include a business-impact line when possible
- If no critical or high issues exist, OMIT the entire banner element

## HTML Components Reference

| Component | Classes |
|-----------|---------|
| Skip link | `.skip-link` (first `<a>` in body, href to `#main-content`) |
| Metric cards | `.stats-grid` > `.stat-card` (`.critical`/`.warning`/`.success`) |
| Badges | `.badge-critical`/`.badge-high`/`.badge-medium`/`.badge-low`/`.badge-info` |
| Alerts | `.alert` + `.alert-danger`/`.alert-warning`/`.alert-info`/`.alert-success` |
| Layered diagram | `.diagram-container` > `.diagram-layers` > `.diagram-layer` |
| Flow diagram | `.flow-diagram` > `.flow-node` + `.flow-arrow` |
| AWS diagram | `.aws-diagram` > `.aws-region` > `.aws-service` |
| Collapsible | `.collapsible` > `.collapsible-header` + `.collapsible-content` |
| Mermaid diagram | `<pre class="mermaid">` (index.html only — ER, sequence, class, state, flowchart) |
| Mermaid fallback | `.mermaid-fallback` with `.mermaid-label` (detail pages only — code block display) |
| Tabs | `.nav-tabs` > `.nav-tab` with `data-tab-id` attributes |
| Tab content | `.tab-content` (`.active` for visible) |

## Required JavaScript (append at end of index.html)

Read `templates/atx-analysis-report/report-scripts.js` and inline it in a `<script>` tag as the last script before `</body>`. Do NOT hardcode from memory — read the file at generation time.

This script handles tab switching, hash-based tab activation, collapsible sections, and mermaid rendering on tab switch.

## Phase 3: Self-Audit (REQUIRED — run after index.html is complete)

After generating the full index.html, the agent MUST perform a self-audit before declaring the report complete. This catches the most common generation errors. Run each check and fix any failures inline.

### Audit 1: Mermaid Syntax Validation

Run the mermaid validation script:
```bash
node templates/atx-analysis-report/validate-mermaid.mjs atx-analysis-report/<project>/index.html
```

This script extracts all `<pre class="mermaid">` blocks and checks for:
1. HTML entities inside mermaid blocks (`&lt;`, `&gt;`, `&amp;`, `&quot;`)
2. Invalid ER diagram attribute types (`guid`, `uniqueidentifier`, etc.)
3. Special characters in sequence diagram participant aliases
4. Unquoted special characters in flowchart/graph node labels
5. Multi-line notes in state diagrams
6. HTML-escaped stereotypes in class diagrams
7. Unquoted ER diagram relationship labels

If the script exits with code 1, fix the reported issues and re-run until it passes.

Additionally, manually verify:
- Every `<pre class="mermaid">` has a matching `</pre>` — count opening and closing tags
- No trailing whitespace on mermaid lines

### Audit 2: Data Consistency Checks

1. **Technical Debt row count** — count the rows in the Technical Debt Inventory table. Compare against the sum of High + Medium + Low stat card values. If they don't match, add the missing rows.
2. **Security alert count** — count the alert divs in the Analysis tab's Security Assessment section. Compare against the Security Findings stat card value. If they don't match, add the missing alerts.
3. **Tab count** — count `<button class="nav-tab` elements (matches both active and inactive tabs). Must be exactly 10 (or 10 + number of extra areas). Verify each has a matching `<div class="tab-content" id="...">`.
4. **Detail page links** — verify every tab except Recommendations & Roadmap has exactly one `<a href="details-*.html"` link and the href matches an actual file in the manifest. Verify the Recommendations tab does NOT have a detail page link (it is AI-generated with no detail page). Also verify each detail page file exists on disk and is non-empty (catches silent Python script failures).

### Audit 3: Structural Checks

1. **Skip link** — verify `<a href="#main-content" class="skip-link">` is the first element after `<body>`.
2. **Critical banner** — if high-severity issues > 0, verify `<div class="critical-banner">` exists between the header and the container. If no high-severity issues, verify it is absent.
3. **Footer metadata** — verify the footer contains: project name, generation date, area count, file count, ATX conversation ID, and agent minutes used. Read these from `atx-analysis-report/<project>/atx-execution-metadata.json`. Example format: `<p>Conversation ID: <code>XXX</code> | Agent Minutes: <code>YYY</code></p>`.
4. **Mermaid.js script** — verify `<script src="mermaid.min.js"></script>` exists, followed by `mermaid.initialize({startOnLoad:false`. Also verify `mermaid.min.js` file exists in the report output directory.
5. **Tab switching JS** — verify the `(function() { ... })();` block with `nav-tab` event listeners and `renderMermaid` function appears as the last `<script>` before `</body>`.
6. **CSS inlined** — verify the `<style>` tag contains the CSS variables (`:root{--primary:`) and is not empty or truncated. Also verify the inlined CSS contains these distinctive selectors from the template: `.mermaid-fallback`, `.tree-diagram`, `.readiness-score`, `.back-link`. If any are missing, the CSS was likely hardcoded from memory instead of read from `templates/atx-analysis-report/assessment-report.css` — re-read the template file and re-inline.

### Audit 4: Content Quality Checks

1. **No placeholder text** — search for `{{`, `}}`, `PLACEHOLDER`, `TODO`, `TBD`, `FIXME` in the generated HTML. Also search for internal generation markers: any `ALL_CAPS_UNDERSCORE` strings 10+ characters long (e.g., `RECO_SPLIT_MARKER`, `TAB_CONTENT_END`) that are not HTML/CSS constants. If found, replace with real content or remove.
2. **No empty tabs** — verify every `.tab-content` div contains at least one `.card` with an `<h2>`, a `.stats-grid` with at least 3 `.stat-card` elements, and a `<table>` with at least 1 data row (beyond the `<thead>`). If a tab has fewer stat cards than its per-tab minimum (see Per-Tab Required Content table), add the missing cards.
3. **Readiness score present** — verify the Overview tab contains a `.readiness-score` div with a numeric score.
4. **Investment Summary present** — verify the Recommendations tab contains 4 `.stat-card` elements for cost metrics.
5. **No duplicate elements** — for each tab, verify: exactly 1 `.stats-grid` (unless the tab has distinct subsections), exactly 1 `.back-link` anchor (except Recommendations which has 0), and no repeated `.stat-card` content (same `.stat-label` text appearing more than once in the same `.stats-grid`). This catches `fsAppend` duplication bugs from area-by-area generation.

### Audit Output

After completing all checks, output a summary:
```
=== Self-Audit Complete ===
✓ Mermaid syntax: X blocks validated
✓ Data consistency: tech debt rows match, security alerts match
✓ Structure: skip link, banner, footer, scripts all present
✓ Content: no placeholders, no empty tabs, readiness score present
```

If any check fails, fix the issue immediately and re-verify. Do NOT declare the report complete until all audit checks pass.
