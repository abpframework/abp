import os
import re

raw = os.environ.get("RAW_NOTES", "")
lines = raw.splitlines()

output = []
seen = set()


def clean_line(text: str) -> str:
    text = text.strip()
    if not text:
        return ""

    # Drop markdown headers/changelog lines.
    if re.match(r"^#+\s", text, flags=re.I):
        return ""
    if re.match(r"^\*\*?\s*full\s+changelog", text, flags=re.I):
        return ""
    if re.match(r"^full\s+changelog", text, flags=re.I):
        return ""

    text = re.sub(r"^[\s\-*•]+", "", text)
    text = re.sub(r"\s+by\s+@?[a-zA-Z0-9_-]+\s+in\s+https?://\S+", "", text)
    text = re.sub(r"\s+by\s+@?[a-zA-Z0-9_-]+\s*$", "", text)
    text = re.sub(r"@([a-zA-Z0-9_-]+)", "", text)
    text = re.sub(r"\s*\([^)]*#\d+\)\s*$", "", text)
    text = re.sub(r"\s+#\d+\s*$", "", text)
    text = re.sub(r"\s+", " ", text).strip(" .:-")

    if len(text) < 8:
        return ""

    # Make user-friendly short title + summary when possible.
    if ":" in text:
        left, right = [p.strip() for p in text.split(":", 1)]
        left = left[:40].rstrip(" .")
        right_words = right.split()
        right = " ".join(right_words[:14]).rstrip(" .")
        text = f"{left}: {right}" if right else left
    else:
        words = text.split()
        if len(words) > 16:
            text = " ".join(words[:16]).rstrip(" .")

    return text


for line in lines:
    cleaned = clean_line(line)
    if not cleaned:
        continue

    # Normalize casing and deduplicate.
    cleaned = cleaned[0].upper() + cleaned[1:] if cleaned else cleaned
    key = cleaned.lower()
    if key in seen:
        continue
    seen.add(key)

    output.append(f"* {cleaned}")
    if len(output) >= 8:
        break

os.makedirs(".tmp", exist_ok=True)
with open(".tmp/final-notes.txt", "w", encoding="utf-8") as f:
    f.write("\n".join(output))
