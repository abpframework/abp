import os
import sys
import re
import json
from openai import OpenAI

client = OpenAI(api_key=os.environ['OPENAI_API_KEY'])

# Regex patterns as constants
SEO_BLOCK_PATTERN = r'```+json\s*//\[doc-seo\]\s*(\{.*?\})\s*```+'
SEO_BLOCK_WITH_BACKTICKS_PATTERN = r'(```+)json\s*//\[doc-seo\]\s*(\{.*?\})\s*\1'

def has_seo_description(content):
    """Check if content already has SEO description with Description field"""
    match = re.search(SEO_BLOCK_PATTERN, content, flags=re.DOTALL)
    
    if not match:
        return False
    
    try:
        json_str = match.group(1)
        seo_data = json.loads(json_str)
        return 'Description' in seo_data and seo_data['Description']
    except json.JSONDecodeError:
        return False

def has_seo_block(content):
    """Check if content has any SEO block (with or without Description)"""
    return bool(re.search(SEO_BLOCK_PATTERN, content, flags=re.DOTALL))

def remove_seo_blocks(content):
    """Remove all SEO description blocks from content"""
    return re.sub(SEO_BLOCK_PATTERN + r'\s*', '', content, flags=re.DOTALL)

def is_content_too_short(content, min_length=200):
    """Check if content is less than minimum length (excluding SEO blocks)"""
    clean_content = remove_seo_blocks(content)
    return len(clean_content.strip()) < min_length

def get_content_preview(content, max_length=1000):
    """Get preview of content for OpenAI (excluding SEO blocks)"""
    clean_content = remove_seo_blocks(content)
    return clean_content[:max_length].strip()

def escape_json_string(text):
    """Escape special characters for JSON"""
    return text.replace('\\', '\\\\').replace('"', '\\"').replace('\n', '\\n')

def create_seo_block(description):
    """Create a new SEO block with the given description"""
    escaped_desc = escape_json_string(description)
    return f'''```json
//[doc-seo]
{{
    "Description": "{escaped_desc}"
}}
```

'''

def generate_description(content, filename):
    """Generate SEO description using OpenAI"""
    try:
        preview = get_content_preview(content)
        
        response = client.chat.completions.create(
            model="gpt-4o-mini",
            messages=[
                {"role": "system", "content": """Create a short and engaging summary (1–2 sentences) for sharing this documentation link on Discord, LinkedIn, Reddit, Twitter and Facebook. Clearly describe what the page explains or teaches.
Highlight the value for developers using ABP Framework.
Be written in a friendly and professional tone.
Stay under 150 characters.
--> https://abp.io/docs/latest <--"""},
                {"role": "user", "content": f"""Generate a concise, informative meta description for this documentation page.

File: {filename}
Content Preview:
{preview}

Requirements:
- Maximum 150 characters

Generate only the description text, nothing else:"""}
            ],
            max_tokens=150,
            temperature=0.7
        )
        
        description = response.choices[0].message.content.strip()
        return description
    except Exception as e:
        print(f"❌ Error generating description: {e}")
        return f"Learn about {os.path.splitext(filename)[0]} in ABP Framework documentation."

def update_seo_description(content, description):
    """Update existing SEO block with new description"""
    match = re.search(SEO_BLOCK_WITH_BACKTICKS_PATTERN, content, flags=re.DOTALL)
    
    if not match:
        return None
    
    backticks = match.group(1)
    json_str = match.group(2)
    
    try:
        seo_data = json.loads(json_str)
        seo_data['Description'] = description
        updated_json = json.dumps(seo_data, indent=4, ensure_ascii=False)
        
        new_block = f'''{backticks}json
//[doc-seo]
{updated_json}
{backticks}'''
        
        return re.sub(SEO_BLOCK_WITH_BACKTICKS_PATTERN, new_block, content, count=1, flags=re.DOTALL)
    except json.JSONDecodeError:
        return None

def add_seo_description(content, description):
    """Add or update SEO description in content"""
    # Try to update existing block first
    updated_content = update_seo_description(content, description)
    if updated_content:
        return updated_content
    
    # No existing block or update failed, add new block at the beginning
    return create_seo_block(description) + content

def is_file_ignored(filepath, ignored_folders):
    """Check if file is in an ignored folder"""
    path_parts = filepath.split('/')
    return any(ignored in path_parts for ignored in ignored_folders)

def get_changed_files():
    """Get changed files from command line or environment variable"""
    if len(sys.argv) > 1:
        return sys.argv[1:]
    
    changed_files_str = os.environ.get('CHANGED_FILES', '')
    return [f.strip() for f in changed_files_str.strip().split('\n') if f.strip()]

def process_file(filepath, ignored_folders):
    """Process a single markdown file. Returns (processed, skipped, skip_reason)"""
    if not filepath.endswith('.md'):
        return False, False, None
    
    # Check if file is in ignored folder
    if is_file_ignored(filepath, ignored_folders):
        print(f"📄 Processing: {filepath}")
        print(f"   🚫 Skipped (ignored folder)\n")
        return False, True, 'ignored'
    
    print(f"📄 Processing: {filepath}")
    
    try:
        # Read file with original line endings
        with open(filepath, 'r', encoding='utf-8', newline='') as f:
            content = f.read()
        
        # Check if content is too short
        if is_content_too_short(content):
            print(f"   ⏭️  Skipped (content less than 200 characters)\n")
            return False, True, 'too_short'
        
        # Check if already has SEO description
        if has_seo_description(content):
            print(f"   ⏭️  Skipped (already has SEO description)\n")
            return False, True, 'has_description'
        
        # Generate description
        filename = os.path.basename(filepath)
        print(f"   🤖 Generating description...")
        description = generate_description(content, filename)
        print(f"   💡 Generated: {description}")
        
        # Add or update SEO description
        if has_seo_block(content):
            print(f"   🔄 Updating existing SEO block...")
        else:
            print(f"   ➕ Adding new SEO block...")
        
        updated_content = add_seo_description(content, description)
        
        # Write back (preserving line endings)
        with open(filepath, 'w', encoding='utf-8', newline='') as f:
            f.write(updated_content)
        
        print(f"   ✅ Updated successfully\n")
        return True, False, None
        
    except Exception as e:
        print(f"   ❌ Error: {e}\n")
        return False, False, None

def save_statistics(processed_count, skipped_count, skipped_too_short, skipped_ignored):
    """Save processing statistics to file"""
    try:
        with open('/tmp/seo_stats.txt', 'w') as f:
            f.write(f"{processed_count}\n{skipped_count}\n{skipped_too_short}\n{skipped_ignored}")
    except Exception as e:
        print(f"⚠️  Warning: Could not save statistics: {e}")

def save_updated_files(updated_files):
    """Save list of updated files"""
    try:
        with open('/tmp/seo_updated_files.txt', 'w') as f:
            f.write('\n'.join(updated_files))
    except Exception as e:
        print(f"⚠️  Warning: Could not save updated files list: {e}")

def main():
    # Get ignored folders from environment
    IGNORED_FOLDERS_STR = os.environ.get('IGNORED_FOLDERS', 'Blog-Posts,Community-Articles,_deleted,_resources')
    IGNORED_FOLDERS = [folder.strip() for folder in IGNORED_FOLDERS_STR.split(',') if folder.strip()]
    
    # Get changed files
    changed_files = get_changed_files()
    
    # Statistics
    processed_count = 0
    skipped_count = 0
    skipped_too_short = 0
    skipped_ignored = 0
    updated_files = []
    
    print("🤖 Processing changed markdown files...\n")
    print(f"� Ignored folders: {', '.join(IGNORED_FOLDERS)}\n")
    
    # Process each file
    for filepath in changed_files:
        processed, skipped, skip_reason = process_file(filepath, IGNORED_FOLDERS)
        
        if processed:
            processed_count += 1
            updated_files.append(filepath)
        elif skipped:
            skipped_count += 1
            if skip_reason == 'too_short':
                skipped_too_short += 1
            elif skip_reason == 'ignored':
                skipped_ignored += 1
    
    # Print summary
    print(f"\n📊 Summary:")
    print(f"   ✅ Updated: {processed_count}")
    print(f"   ⏭️  Skipped (total): {skipped_count}")
    print(f"   ⏭️  Skipped (too short): {skipped_too_short}")
    print(f"   🚫 Skipped (ignored folder): {skipped_ignored}")
    
    # Save statistics
    save_statistics(processed_count, skipped_count, skipped_too_short, skipped_ignored)
    save_updated_files(updated_files)

if __name__ == '__main__':
    main()
