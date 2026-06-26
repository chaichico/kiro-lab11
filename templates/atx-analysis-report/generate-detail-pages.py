#!/usr/bin/env python3
"""
Generate detail pages for AWS Transform assessment reports.

Reads all input files from an AWS Transform CLI output directory,
converts markdown to HTML, and generates self-contained detail-*.html
pages with collapsible sections per file.

Usage:
    python templates/atx-analysis-report/generate-detail-pages.py <input_dir> <output_dir> [--project-title TITLE]

Example:
    python templates/atx-analysis-report/generate-detail-pages.py /path/to/input /path/to/output
    python templates/atx-analysis-report/generate-detail-pages.py /path/to/input /path/to/output --project-title "My Project"
"""

import argparse
import json
import os
import re
import html
from datetime import datetime
from pathlib import Path

SCRIPT_DIR = Path(__file__).parent
CSS_FILE = SCRIPT_DIR / "assessment-report.css"

# Area emoji mapping (fallback to 📁 for unknown areas)
AREA_EMOJIS = {
    "overview": "📋", "architecture": "🏗️", "analysis": "📊",
    "metrics": "📊", "technical-debt": "⚠️", "behavior": "⚙️",
    "migration": "🚀", "reference": "📚", "diagrams": "📈",
    "specialized": "🔧", "logs": "📝", "security": "🔒",
}


def read_css():
    """Read the shared CSS file."""
    if CSS_FILE.exists():
        return CSS_FILE.read_text(encoding="utf-8")
    print(f"WARNING: CSS file not found at {CSS_FILE}, using empty styles")
    return ""


def markdown_to_html(text):
    """Convert markdown to HTML. Handles nested lists, multi-line blockquotes,
    and mermaid code blocks with a labeled fallback display."""
    lines = text.split("\n")
    result = []
    in_code_block = False
    code_lang = ""
    code_lines = []
    in_table = False
    table_lines = []
    in_list = False
    list_type = None
    list_lines = []
    in_blockquote = False
    bq_lines = []

    def flush_blockquote():
        nonlocal in_blockquote, bq_lines
        if in_blockquote and bq_lines:
            result.append("<blockquote>" + " ".join(inline_md(l) for l in bq_lines) + "</blockquote>")
            bq_lines = []
            in_blockquote = False

    def flush_list():
        nonlocal in_list, list_type, list_lines
        if in_list and list_lines:
            tag = list_type
            result.append(f"<{tag}>")
            for li in list_lines:
                result.append(f"  <li>{inline_md(li)}</li>")
            result.append(f"</{tag}>")
            list_lines = []
            in_list = False
            list_type = None

    def flush_table():
        nonlocal in_table, table_lines
        if in_table and table_lines:
            result.append(render_table(table_lines))
            table_lines = []
            in_table = False

    def render_table(tlines):
        rows = []
        for tl in tlines:
            cells = [c.strip() for c in tl.strip().strip("|").split("|")]
            rows.append(cells)
        if len(rows) < 2:
            return "<p>" + html.escape("|".join(rows[0])) + "</p>"
        header = rows[0]
        data_rows = [r for i, r in enumerate(rows) if i > 0 and not re.match(r"^[\s\-:|]+$", "|".join(r))]
        h = "<table><thead><tr>" + "".join(f"<th>{inline_md(c)}</th>" for c in header) + "</tr></thead><tbody>"
        for dr in data_rows:
            h += "<tr>" + "".join(f"<td>{inline_md(c)}</td>" for c in dr) + "</tr>"
        h += "</tbody></table>"
        return h

    def inline_md(s):
        s = html.escape(s)
        s = re.sub(r"`([^`]+)`", r"<code>\1</code>", s)
        s = re.sub(r"\*\*(.+?)\*\*", r"<strong>\1</strong>", s)
        s = re.sub(r"__(.+?)__", r"<strong>\1</strong>", s)
        s = re.sub(r"\*(.+?)\*", r"<em>\1</em>", s)
        s = re.sub(r"_(.+?)_", r"<em>\1</em>", s)
        s = re.sub(r"\[([^\]]+)\]\(([^)]+)\)", r'<a href="\2">\1</a>', s)
        return s

    for line in lines:
        # Fenced code blocks
        if re.match(r"^```", line):
            if in_code_block:
                escaped = html.escape("\n".join(code_lines))
                if code_lang == "mermaid":
                    result.append(f'<div class="mermaid-fallback"><span class="mermaid-label">Mermaid Diagram</span>\n{escaped}</div>')
                else:
                    result.append(f"<pre><code>{escaped}</code></pre>")
                code_lines = []
                code_lang = ""
                in_code_block = False
                continue
            else:
                flush_blockquote()
                flush_list()
                flush_table()
                code_lang = line.strip("`").strip()
                in_code_block = True
                continue
        if in_code_block:
            code_lines.append(line)
            continue

        # Table detection
        if "|" in line and re.match(r"^\s*\|", line):
            flush_blockquote()
            flush_list()
            in_table = True
            table_lines.append(line)
            continue
        elif in_table:
            flush_table()

        # Blockquote (multi-line: consecutive > lines merge)
        if line.startswith(">"):
            flush_list()
            flush_table()
            text = line.lstrip("> ")
            if not in_blockquote:
                in_blockquote = True
            bq_lines.append(text)
            continue
        elif in_blockquote:
            flush_blockquote()

        # Unordered list
        ul_match = re.match(r"^(\s*)[-*+]\s+(.+)$", line)
        if ul_match:
            flush_table()
            flush_blockquote()
            if not in_list or list_type != "ul":
                flush_list()
                in_list = True
                list_type = "ul"
            list_lines.append(ul_match.group(2))
            continue

        # Ordered list
        ol_match = re.match(r"^(\s*)\d+\.\s+(.+)$", line)
        if ol_match:
            flush_table()
            flush_blockquote()
            if not in_list or list_type != "ol":
                flush_list()
                in_list = True
                list_type = "ol"
            list_lines.append(ol_match.group(2))
            continue

        # If we were in a list and hit a non-list line, flush
        if in_list:
            flush_list()

        # Headings
        h_match = re.match(r"^(#{1,6})\s+(.+)$", line)
        if h_match:
            level = len(h_match.group(1))
            result.append(f"<h{level}>{inline_md(h_match.group(2))}</h{level}>")
            continue

        # Horizontal rule
        if re.match(r"^---+$", line.strip()):
            result.append("<hr>")
            continue

        # Empty line
        if not line.strip():
            result.append("")
            continue

        # Regular paragraph
        result.append(f"<p>{inline_md(line)}</p>")

    # Flush remaining
    if in_code_block:
        escaped = html.escape("\n".join(code_lines))
        if code_lang == "mermaid":
            result.append(f'<div class="mermaid-fallback"><span class="mermaid-label">Mermaid Diagram</span>\n{escaped}</div>')
        else:
            result.append(f"<pre><code>{escaped}</code></pre>")
    flush_blockquote()
    flush_list()
    flush_table()

    return "\n".join(result)


def discover_areas(input_dir):
    """Discover areas from the input directory structure.
    
    Returns dict: { area_slug: { 'title': str, 'files': [Path, ...] } }
    """
    input_path = Path(input_dir)
    areas = {}

    # Find Documentation folder(s)
    doc_dirs = sorted([d for d in input_path.iterdir() if d.is_dir() and d.name.lower().startswith("documentation")])
    
    if not doc_dirs:
        # No Documentation folder — treat all subfolders as areas
        doc_dirs = [input_path]

    # Root-level markdown files → overview
    root_files = sorted([f for f in input_path.glob("*.md")])
    for doc_dir in doc_dirs:
        root_files.extend(sorted([f for f in doc_dir.glob("*.md")]))
    
    if root_files:
        areas["overview"] = {
            "title": "Overview",
            "files": list(dict.fromkeys(root_files)),  # dedupe preserving order
        }

    # Area subfolders
    for doc_dir in doc_dirs:
        for sub in sorted(doc_dir.iterdir()):
            if not sub.is_dir():
                continue
            slug = sub.name.lower().replace(" ", "-").replace("_", "-")
            files = sorted(sub.rglob("*"), key=lambda f: str(f))
            files = [f for f in files if f.is_file()]
            if files:
                if slug in areas:
                    areas[slug]["files"].extend(files)
                else:
                    title = sub.name.replace("-", " ").replace("_", " ").title()
                    areas[slug] = {"title": title, "files": files}

    # Logs
    logs_dir = input_path / "logs"
    if logs_dir.exists():
        log_files = sorted(logs_dir.rglob("*"))
        log_files = [f for f in log_files if f.is_file()]
        if log_files:
            areas["logs"] = {"title": "Logs", "files": log_files}

    return areas


def read_file_content(filepath):
    """Read file content, handling encoding issues."""
    for enc in ("utf-8", "latin-1", "cp1252"):
        try:
            return filepath.read_text(encoding=enc)
        except (UnicodeDecodeError, ValueError):
            continue
    return f"[Binary file — cannot display: {filepath.name}]"


def generate_detail_page(area_slug, area_info, all_areas, input_dir, css, project_title):
    """Generate a single detail page HTML."""
    input_path = Path(input_dir)
    emoji = AREA_EMOJIS.get(area_slug, "📁")
    title = area_info["title"]
    now = datetime.now().strftime("%Y-%m-%d %H:%M")

    # Build cross-page nav
    nav_links = ['<a href="index.html">📋 Main Report</a>']
    for slug, info in all_areas.items():
        e = AREA_EMOJIS.get(slug, "📁")
        cls = ' class="current"' if slug == area_slug else ""
        nav_links.append(f'<a href="details-{slug}.html"{cls}>{e} {info["title"]}</a>')
    nav_html = "\n    ".join(nav_links)

    # Build collapsible sections
    sections = []
    for filepath in area_info["files"]:
        try:
            rel_path = filepath.relative_to(input_path)
        except ValueError:
            rel_path = filepath.name
        
        content = read_file_content(filepath)
        
        # Convert markdown files to HTML, escape others
        if filepath.suffix.lower() in (".md", ".markdown"):
            rendered = markdown_to_html(content)
        elif filepath.suffix.lower() in (".html", ".htm"):
            rendered = content  # already HTML
        else:
            rendered = f"<pre><code>{html.escape(content)}</code></pre>"

        sections.append(f"""<div class="collapsible">
  <div class="collapsible-header">📄 {html.escape(str(rel_path))}</div>
  <div class="collapsible-content">
    {rendered}
  </div>
</div>""")

    sections_html = "\n".join(sections)
    file_count = len(area_info["files"])

    return f"""<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>{html.escape(title)} — {html.escape(project_title)} Detail</title>
  <style>
{css}
  </style>
</head>
<body>

<a href="#main-content" class="skip-link">Skip to main content</a>

  <div class="header">
    <div class="header-content">
      <h1>{emoji} {html.escape(title)}</h1>
      <div class="subtitle">{html.escape(project_title)} — Full Detail View ({file_count} files)</div>
    </div>
  </div>

  <div class="page-nav">
    {nav_html}
  </div>

  <div class="container" id="main-content">
    <a href="index.html" class="back-link">← Back to Main Report</a>

    <div class="card">
      <h2>{emoji} {html.escape(title)} — Complete Content</h2>
      <p>This page contains the full content of all {file_count} input file(s) for this area. Each file is in a collapsible section below.</p>
    </div>

    {sections_html}
  </div>

  <div class="footer">
    <p>Generated on {now} | {html.escape(project_title)} — {html.escape(title)} Detail Page</p>
  </div>

  <script>
  (function() {{
    document.querySelectorAll('.collapsible-header').forEach(function(h) {{
      function toggle() {{
        h.classList.toggle('expanded');
        h.nextElementSibling.classList.toggle('show');
      }}
      h.addEventListener('click', toggle);
      h.addEventListener('keydown', function(e) {{
        if (e.key === 'Enter' || e.key === ' ') {{ e.preventDefault(); toggle(); }}
      }});
    }});
  }})();
  </script>
</body>
</html>"""


def main():
    parser = argparse.ArgumentParser(description="Generate detail pages for assessment reports")
    parser.add_argument("input_dir", help="Path to input directory (e.g., /path/to/project-input)")
    parser.add_argument("output_dir", help="Path to output directory (e.g., /path/to/project-output)")
    parser.add_argument("--project-title", default=None, help="Project title for headers (defaults to folder name)")
    args = parser.parse_args()

    input_dir = Path(args.input_dir)
    output_dir = Path(args.output_dir)

    if not input_dir.exists():
        print(f"ERROR: Input directory not found: {input_dir}")
        return 1

    project_title = args.project_title or input_dir.name
    output_dir.mkdir(parents=True, exist_ok=True)

    css = read_css()
    areas = discover_areas(input_dir)

    if not areas:
        print(f"ERROR: No areas discovered in {input_dir}")
        return 1

    print(f"Project: {project_title}")
    print(f"Input:   {input_dir}")
    print(f"Output:  {output_dir}")
    print(f"Areas discovered: {len(areas)}")
    for slug, info in areas.items():
        emoji = AREA_EMOJIS.get(slug, "📁")
        print(f"  {emoji} {info['title']} ({len(info['files'])} files) → details-{slug}.html")

    # Generate detail pages
    generated = []
    total_files = 0
    for slug, info in areas.items():
        page_html = generate_detail_page(slug, info, areas, input_dir, css, project_title)
        out_file = output_dir / f"details-{slug}.html"
        out_file.write_text(page_html, encoding="utf-8")
        generated.append(f"details-{slug}.html")
        total_files += len(info["files"])
        print(f"  ✓ {out_file.name} ({len(info['files'])} files embedded)")

    # Write manifest for the agent to read
    manifest_path = output_dir / "detail-pages-manifest.json"
    manifest = {
        "project": project_title,
        "generated_at": datetime.now().isoformat(),
        "input_dir": str(input_dir),
        "total_input_files": total_files,
        "areas": {
            slug: {
                "title": info["title"],
                "emoji": AREA_EMOJIS.get(slug, "📁"),
                "file_count": len(info["files"]),
                "detail_page": f"details-{slug}.html",
                "files": [str(f) for f in info["files"]],
            }
            for slug, info in areas.items()
        },
        "detail_pages": generated,
    }
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")
    print(f"\n✓ Manifest: {manifest_path.name}")
    print(f"✓ Done: {len(generated)} detail pages, {total_files} input files embedded")
    print(f"\nThe agent can now read {manifest_path.name} to build index.html tab navigation and summaries.")
    return 0


if __name__ == "__main__":
    exit(main())
