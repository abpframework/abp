import subprocess
import re
import os
import sys
import xml.etree.ElementTree as ET


HEADER = "# Package Version Changes\n"
DOC_PATH = os.environ.get("DOC_PATH", "docs/en/package-version-changes.md")


def get_version():
    """Read the current version from common.props."""
    try:
        tree = ET.parse("common.props")
        root = tree.getroot()
        version_elem = root.find(".//Version")
        if version_elem is not None:
            return version_elem.text
    except FileNotFoundError:
        print("Error: 'common.props' file not found.", file=sys.stderr)
    except ET.ParseError as ex:
        print(f"Error: Failed to parse 'common.props': {ex}", file=sys.stderr)
    return None


def get_diff(base_ref):
    """Get diff of Directory.Packages.props against the base branch."""
    result = subprocess.run(
        ["git", "diff", f"origin/{base_ref}", "--", "Directory.Packages.props"],
        capture_output=True,
        text=True,
    )
    if result.returncode != 0:
        raise RuntimeError(
            f"Failed to get diff for base ref 'origin/{base_ref}': {result.stderr}"
        )
    return result.stdout


def get_existing_doc_from_base(base_ref):
    """Read the existing document from the base branch."""
    result = subprocess.run(
        ["git", "show", f"origin/{base_ref}:{DOC_PATH}"],
        capture_output=True,
        text=True,
    )
    if result.returncode == 0:
        return result.stdout
    return ""


def parse_diff_packages(lines, prefix):
    """Parse package versions from diff lines with the given prefix (+ or -)."""
    packages = {}
    # Use separate patterns to handle different attribute orders
    include_pattern = re.compile(r'Include="([^"]+)"')
    version_pattern = re.compile(r'Version="([^"]+)"')
    for line in lines:
        if line.startswith(prefix) and "PackageVersion" in line and not line.startswith(prefix * 3):
            include_match = include_pattern.search(line)
            version_match = version_pattern.search(line)
            if include_match and version_match:
                packages[include_match.group(1)] = version_match.group(1)
    return packages


def classify_changes(old_packages, new_packages, pr_number):
    """Classify diff into updated, added, and removed with PR attribution."""
    updated = {}
    added = {}
    removed = {}

    all_packages = sorted(set(list(old_packages.keys()) + list(new_packages.keys())))

    for pkg in all_packages:
        if pkg in old_packages and pkg in new_packages:
            if old_packages[pkg] != new_packages[pkg]:
                updated[pkg] = (old_packages[pkg], new_packages[pkg], pr_number)
        elif pkg in new_packages:
            added[pkg] = (new_packages[pkg], pr_number)
        else:
            removed[pkg] = (old_packages[pkg], pr_number)

    return updated, added, removed


def parse_existing_section(section_text):
    """Parse an existing markdown section to extract package records with PR info."""
    updated = {}
    added = {}
    removed = {}

    mode = "updated"
    for line in section_text.split("\n"):
        if "**Added:**" in line:
            mode = "added"
            continue
        if "**Removed:**" in line:
            mode = "removed"
            continue
        if not line.startswith("|") or line.startswith("| Package") or line.startswith("|---"):
            continue

        parts = [p.strip() for p in line.split("|")[1:-1]]
        if mode == "updated" and len(parts) >= 3:
            pr = parts[3] if len(parts) >= 4 else ""
            updated[parts[0]] = (parts[1], parts[2], pr)
        elif len(parts) >= 2:
            pr = parts[2] if len(parts) >= 3 else ""
            if mode == "added":
                added[parts[0]] = (parts[1], pr)
            else:
                removed[parts[0]] = (parts[1], pr)

    return updated, added, removed


def merge_prs(existing_pr, new_pr):
    """Merge PR numbers, avoiding duplicates."""
    if not existing_pr or not existing_pr.strip():
        return new_pr
    if not new_pr or not new_pr.strip():
        return existing_pr
    
    # Parse existing PRs
    existing_prs = [p.strip() for p in existing_pr.split(",") if p.strip()]
    # Add new PR if not already present
    if new_pr not in existing_prs:
        existing_prs.append(new_pr)
    return ", ".join(existing_prs)


def merge_changes(existing, new):
    """Merge new changes into existing records for the same version."""
    ex_updated, ex_added, ex_removed = existing
    new_updated, new_added, new_removed = new

    merged_updated = dict(ex_updated)
    merged_added = dict(ex_added)
    merged_removed = dict(ex_removed)

    for pkg, (old_ver, new_ver, pr) in new_updated.items():
        if pkg in merged_updated:
            existing_old_ver, existing_new_ver, existing_pr = merged_updated[pkg]
            merged_pr = merge_prs(existing_pr, pr)
            merged_updated[pkg] = (existing_old_ver, new_ver, merged_pr)
        elif pkg in merged_added:
            existing_ver, existing_pr = merged_added[pkg]
            merged_pr = merge_prs(existing_pr, pr)
            # Convert added to updated since the version changed again
            del merged_added[pkg]
            merged_updated[pkg] = (existing_ver, new_ver, merged_pr)
        else:
            merged_updated[pkg] = (old_ver, new_ver, pr)

    for pkg, (ver, pr) in new_added.items():
        if pkg in merged_removed:
            removed_ver, removed_pr = merged_removed.pop(pkg)
            merged_pr = merge_prs(removed_pr, pr)
            merged_updated[pkg] = (removed_ver, ver, merged_pr)
        elif pkg in merged_added:
            existing_ver, existing_pr = merged_added[pkg]
            merged_pr = merge_prs(existing_pr, pr)
            merged_added[pkg] = (ver, merged_pr)
        else:
            merged_added[pkg] = (ver, pr)

    for pkg, (ver, pr) in new_removed.items():
        if pkg in merged_added:
            existing_ver, existing_pr = merged_added[pkg]
            # Only delete if versions match (added then removed the same version)
            if existing_ver == ver:
                del merged_added[pkg]
            else:
                # Version changed between add and remove, convert to updated then removed
                del merged_added[pkg]
                merged_removed[pkg] = (ver, merge_prs(existing_pr, pr))
        elif pkg in merged_updated:
            old_ver, new_ver, existing_pr = merged_updated.pop(pkg)
            merged_pr = merge_prs(existing_pr, pr)
            # Only keep as removed if the final state is different from original
            merged_removed[pkg] = (old_ver, merged_pr)
        else:
            merged_removed[pkg] = (ver, pr)

    # Remove updated entries where old and new versions are the same
    merged_updated = {k: v for k, v in merged_updated.items() if v[0] != v[1]}
    
    # Remove added entries that are also in removed with the same version
    for pkg in list(merged_added.keys()):
        if pkg in merged_removed:
            added_ver, added_pr = merged_added[pkg]
            removed_ver, removed_pr = merged_removed[pkg]
            if added_ver == removed_ver:
                # Package was added and removed at the same version, cancel out
                del merged_added[pkg]
                del merged_removed[pkg]

    return merged_updated, merged_added, merged_removed


def render_section(version, updated, added, removed):
    """Render a version section as markdown."""
    lines = [f"## {version}\n"]

    if updated:
        lines.append("| Package | Old Version | New Version | PR |")
        lines.append("|---------|-------------|-------------|-----|")
        for pkg in sorted(updated):
            old_ver, new_ver, pr = updated[pkg]
            lines.append(f"| {pkg} | {old_ver} | {new_ver} | {pr} |")
        lines.append("")

    if added:
        lines.append("**Added:**\n")
        lines.append("| Package | Version | PR |")
        lines.append("|---------|---------|-----|")
        for pkg in sorted(added):
            ver, pr = added[pkg]
            lines.append(f"| {pkg} | {ver} | {pr} |")
        lines.append("")

    if removed:
        lines.append("**Removed:**\n")
        lines.append("| Package | Version | PR |")
        lines.append("|---------|---------|-----|")
        for pkg in sorted(removed):
            ver, pr = removed[pkg]
            lines.append(f"| {pkg} | {ver} | {pr} |")
        lines.append("")

    return "\n".join(lines)


def parse_document(content):
    """Split document into a list of (version, section_text) tuples."""
    sections = []
    current_version = None
    current_lines = []

    for line in content.split("\n"):
        match = re.match(r"^## (.+)$", line)
        if match:
            if current_version:
                sections.append((current_version, "\n".join(current_lines)))
            current_version = match.group(1).strip()
            current_lines = [line]
        elif current_version:
            current_lines.append(line)

    if current_version:
        sections.append((current_version, "\n".join(current_lines)))

    return sections


def main():
    if len(sys.argv) < 3:
        print("Usage: update_dependency_changes.py <base-ref> <pr-number>")
        sys.exit(1)

    base_ref = sys.argv[1]
    pr_arg = sys.argv[2]
    
    # Validate PR number is numeric
    if not re.fullmatch(r"\d+", pr_arg):
        print("Invalid PR number; must be numeric.")
        sys.exit(1)
    
    # Validate base_ref doesn't contain dangerous characters
    if not re.fullmatch(r"[a-zA-Z0-9/_.-]+", base_ref):
        print("Invalid base ref; contains invalid characters.")
        sys.exit(1)
    
    pr_number = f"#{pr_arg}"

    version = get_version()
    if not version:
        print("Could not read version from common.props.")
        sys.exit(1)

    diff = get_diff(base_ref)
    if not diff:
        print("No diff found for Directory.Packages.props.")
        sys.exit(0)

    diff_lines = diff.split("\n")
    old_packages = parse_diff_packages(diff_lines, "-")
    new_packages = parse_diff_packages(diff_lines, "+")

    new_updated, new_added, new_removed = classify_changes(old_packages, new_packages, pr_number)

    if not new_updated and not new_added and not new_removed:
        print("No package version changes detected.")
        sys.exit(0)

    # Load existing document from the base branch
    existing_content = get_existing_doc_from_base(base_ref)
    sections = parse_document(existing_content) if existing_content else []

    # Find existing section for this version
    version_index = None
    for i, (v, _) in enumerate(sections):
        if v == version:
            version_index = i
            break

    if version_index is not None:
        existing = parse_existing_section(sections[version_index][1])
        merged = merge_changes(existing, (new_updated, new_added, new_removed))
        section_text = render_section(version, *merged)
        sections[version_index] = (version, section_text)
    else:
        section_text = render_section(version, new_updated, new_added, new_removed)
        sections.insert(0, (version, section_text))

    # Write document
    doc_dir = os.path.dirname(DOC_PATH)
    if doc_dir:
        os.makedirs(doc_dir, exist_ok=True)
    with open(DOC_PATH, "w") as f:
        f.write(HEADER + "\n")
        for _, text in sections:
            f.write(text.rstrip("\n") + "\n\n")

    print(f"Updated {DOC_PATH} for version {version}")


if __name__ == "__main__":
    main()
