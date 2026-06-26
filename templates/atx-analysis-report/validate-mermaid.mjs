#!/usr/bin/env node
/**
 * validate-mermaid.mjs — Extract and validate mermaid diagram blocks from an HTML file.
 *
 * Pattern-based syntax validation — no external dependencies required.
 * Catches the most common mermaid v11 syntax errors before the report is opened.
 *
 * Exit code 0 = all diagrams valid, 1 = one or more failures.
 *
 * Usage:
 *   node templates/atx-analysis-report/validate-mermaid.mjs <html-file>
 *
 * Example:
 *   node templates/atx-analysis-report/validate-mermaid.mjs atx-analysis-report/<project>/index.html
 */

import { readFileSync } from "fs";

const htmlFile = process.argv[2];
if (!htmlFile) {
  console.error("Usage: node validate-mermaid.mjs <html-file>");
  process.exit(1);
}

const html = readFileSync(htmlFile, "utf-8");

// Extract mermaid blocks using regex
const blocks = [];
const regex = /<pre class="mermaid">\s*\n?([\s\S]*?)<\/pre>/g;
let match;
while ((match = regex.exec(html)) !== null) {
  blocks.push(match[1].trim());
}

if (blocks.length === 0) {
  console.log("No mermaid blocks found in", htmlFile);
  process.exit(0);
}

console.log(`Found ${blocks.length} mermaid block(s) in ${htmlFile}\n`);

let failures = 0;

for (let i = 0; i < blocks.length; i++) {
  const block = blocks[i];
  const lines = block.split("\n");
  const diagramType = lines[0].trim().split(/\s/)[0];
  const errors = [];

  // Check 1: HTML entities inside mermaid blocks
  if (/&lt;|&gt;|&amp;|&quot;|&#\d+;/.test(block)) {
    errors.push(
      "Contains HTML entities (&lt; &gt; &amp; &quot;) — mermaid needs raw text, not HTML-escaped"
    );
  }

  // Check 2: ER diagram invalid types
  if (diagramType === "erDiagram") {
    const attrPattern = /^\s{4,}(\w+)\s+\w+/gm;
    let attrMatch;
    const invalidTypes = new Set();
    const validTypes = [
      "string", "int", "integer", "float", "double", "boolean",
      "bool", "date", "datetime", "number", "bigint", "text",
    ];
    while ((attrMatch = attrPattern.exec(block)) !== null) {
      const type = attrMatch[1].toLowerCase();
      if (!validTypes.includes(type)) {
        invalidTypes.add(attrMatch[1]);
      }
    }
    if (invalidTypes.size > 0) {
      errors.push(
        `Possibly invalid ER attribute types: ${[...invalidTypes].join(", ")} — standard types: string, int, float, boolean, date, datetime`
      );
    }
    // Check 2b: ER relationship labels must be quoted
    const relPattern = /\|\|--|o\{|o\||\}\||\|o|\}\{/;
    const relLines = lines.filter((l) => relPattern.test(l));
    for (const rl of relLines) {
      // Match lines with : followed by unquoted label
      const labelMatch = rl.match(/:\s*([^"'\s].*)$/);
      if (labelMatch) {
        const label = labelMatch[1].trim();
        if (label && !label.startsWith('"') && !label.startsWith("'")) {
          errors.push(
            `Unquoted ER relationship label "${label}" — wrap in double quotes: : "${label}"`
          );
        }
      }
    }
  }

  // Check 3: Sequence diagram participant aliases with special chars
  if (diagramType === "sequenceDiagram") {
    const participantPattern = /participant\s+\w+\s+as\s+(.+)/g;
    let pMatch;
    while ((pMatch = participantPattern.exec(block)) !== null) {
      const alias = pMatch[1].trim();
      if (/[()\/\[\].]/.test(alias)) {
        errors.push(
          `Participant alias "${alias}" contains special chars — remove ( ) / [ ] .`
        );
      }
    }
    // Check message text for brackets
    const msgPattern = /(?:--?>>?|-->>)\s*\w+:\s*(.+)/g;
    let mMatch;
    while ((mMatch = msgPattern.exec(block)) !== null) {
      const msg = mMatch[1].trim();
      if (/[\[\]]/.test(msg)) {
        errors.push(
          `Message text "${msg}" contains brackets — remove [ ]`
        );
      }
    }
  }

  // Check 4: Flowchart/graph unquoted special chars in node labels
  if (diagramType === "flowchart" || diagramType === "graph") {
    // Unquoted square bracket nodes: A[text with /special]
    const unquotedSquare = /\w+\[([^\]"]+)\]/g;
    let nMatch;
    while ((nMatch = unquotedSquare.exec(block)) !== null) {
      const label = nMatch[1];
      if (/[\/\.+&<>()]/.test(label)) {
        errors.push(
          `Node label "${label}" has special chars but is not quoted — use ["${label}"]`
        );
      }
    }
    // Unquoted curly bracket nodes (decision): A{text with /special}
    const unquotedCurly = /\w+\{([^}"]+)\}/g;
    while ((nMatch = unquotedCurly.exec(block)) !== null) {
      const label = nMatch[1];
      if (/[\/\.+&<>()]/.test(label)) {
        errors.push(
          `Decision label "${label}" has special chars but is not quoted — use {"${label}"}`
        );
      }
    }
    // Cylinder nodes without quotes: SQL[(text)]
    const cylinderUnquoted = /\w+\[\(([^)"]+)\)\]/g;
    while ((nMatch = cylinderUnquoted.exec(block)) !== null) {
      const label = nMatch[1];
      if (/\s/.test(label)) {
        errors.push(
          `Cylinder label "${label}" should be quoted — use [("${label}")]`
        );
      }
    }
  }

  // Check 5: State diagram multi-line notes
  if (
    diagramType === "stateDiagram-v2" ||
    diagramType === "stateDiagram"
  ) {
    if (/\bnote\s+(?:right|left)\s+of\s+\w+\s*\n\s+\S/.test(block)) {
      errors.push(
        "Multi-line note block detected — convert to single-line: note right of X : text"
      );
    }
  }

  // Check 6: Class diagram stereotypes (should be avoided entirely)
  if (diagramType === "classDiagram") {
    if (/<<\w+>>/.test(block) || /&lt;&lt;\w+&gt;&gt;/.test(block)) {
      errors.push(
        'Stereotype <<...>> detected — avoid stereotypes in HTML. Rename class instead (e.g., class RatingEnum)'
      );
    }
  }

  // Report
  const label = `Block ${i + 1} (${diagramType})`;
  if (errors.length > 0) {
    console.log(`✗ ${label}`);
    errors.forEach((e) => console.log(`    → ${e}`));
    failures++;
  } else {
    console.log(`✓ ${label}`);
  }
}

console.log(
  `\n${blocks.length - failures}/${blocks.length} blocks passed validation`
);
if (failures > 0) {
  console.log(`${failures} block(s) have issues — fix before publishing`);
  process.exit(1);
} else {
  console.log("All mermaid blocks passed validation ✓");
  process.exit(0);
}
