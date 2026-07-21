import os
import re
from packaging.version import Version, InvalidVersion

studio_ver = os.environ["STUDIO_VERSION"]
abp_ver = os.environ["ABP_VERSION"]
file_path = "docs/en/studio/version-mapping.md"

try:
    studio = Version(studio_ver)
except InvalidVersion:
    print(f"❌ Invalid Studio version: {studio_ver}")
    raise SystemExit(1)

with open(file_path, "r") as f:
    lines = f.readlines()

# Find table start (skip SEO and headers)
table_start = 0
table_end = 0
for i, line in enumerate(lines):
    if line.strip().startswith("|") and "**ABP Studio Version**" in line:
        table_start = i
    elif table_start > 0 and line.strip() and not line.strip().startswith("|"):
        table_end = i
        break

if table_start == 0:
    print("❌ Could not find version mapping table")
    raise SystemExit(1)

# If no end found, table goes to end of file
if table_end == 0:
    table_end = len(lines)

# Extract sections
before_table = lines[:table_start]
table_header = lines[table_start : table_start + 2]
data_rows = [l for l in lines[table_start + 2 : table_end] if l.strip().startswith("|")]
after_table = lines[table_end:]

new_rows = []
handled = False


def parse_version_range(version_str):
    """Parse '2.1.5 - 2.1.9' or '2.1.5' into (start, end)"""
    version_str = version_str.strip()

    if "–" in version_str or "-" in version_str:
        parts = re.split(r"\s*[–-]\s*", version_str)
        if len(parts) == 2:
            try:
                return Version(parts[0].strip()), Version(parts[1].strip())
            except InvalidVersion:
                return None, None

    try:
        v = Version(version_str)
        return v, v
    except InvalidVersion:
        return None, None


def format_row(studio_range, abp_version):
    """Format a table row with proper spacing"""
    return f"| {studio_range:<22} | {abp_version:<27} |\n"


# Process existing rows
for row in data_rows:
    match = re.match(r"\|\s*(.+?)\s*\|\s*(.+?)\s*\|", row)
    if not match:
        continue

    existing_studio_range = match.group(1).strip()
    existing_abp = match.group(2).strip()

    if existing_abp != abp_ver:
        new_rows.append(row)
        continue

    start_ver, end_ver = parse_version_range(existing_studio_range)

    if start_ver is None or end_ver is None:
        new_rows.append(row)
        continue

    if start_ver <= studio <= end_ver:
        print(f"✅ Studio version {studio_ver} already covered in range {existing_studio_range}")
        handled = True
        new_rows.append(row)
    elif end_ver < studio:
        if (
            start_ver.major == studio.major
            and start_ver.minor == studio.minor
            and studio.micro <= end_ver.micro + 5
        ):
            new_range = f"{start_ver} - {studio}"
            new_rows.append(format_row(new_range, abp_ver))
            print(f"✅ Extended range: {new_range}")
            handled = True
        else:
            new_rows.append(row)
    else:
        new_rows.append(row)

if not handled:
    new_row = format_row(str(studio), abp_ver)
    new_rows.insert(0, new_row)
    print(f"✅ Added new mapping: {studio_ver} -> {abp_ver}")

with open(file_path, "w") as f:
    f.writelines(before_table)
    f.writelines(table_header)
    f.writelines(new_rows)
    f.writelines(after_table)
