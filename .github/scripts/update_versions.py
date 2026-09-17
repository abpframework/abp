import os
import json
import re
import xml.etree.ElementTree as ET
from github import Github

def get_target_release_branch(version):
    """ 
    Extracts the first two numbers from the release version (`9.0.5` → `rel-9.0`) 
    to determine the corresponding `rel-x.x` branch.
    """
    match = re.match(r"(\d+)\.(\d+)\.\d+", version)
    if not match:
        raise ValueError(f"Invalid version format: {version}")

    major, minor = match.groups()
    target_branch = f"rel-{major}.{minor}"
    return target_branch

def get_version_from_common_props(branch):
    """ 
    Retrieves `Version` and `LeptonXVersion` from the `common.props` file in the specified branch. 
    """
    g = Github(os.environ["GITHUB_TOKEN"])
    repo = g.get_repo("abpframework/abp")

    try:
        file_content = repo.get_contents("common.props", ref=branch)
        common_props_content = file_content.decoded_content.decode("utf-8")

        root = ET.fromstring(common_props_content)
        version = root.find(".//Version").text
        leptonx_version = root.find(".//LeptonXVersion").text

        return version, leptonx_version
    except Exception as e:
        raise FileNotFoundError(f"common.props not found in branch {branch}: {e}")

def update_latest_versions():
    """ 
    Updates `latest-versions.json` based on the most relevant release branch. 
    """
    # Get the release version from GitHub reference
    release_version = os.environ["GITHUB_REF"].split("/")[-1]  # Example: "refs/tags/v9.0.5" → "v9.0.5"
    if release_version.startswith("v"):
        release_version = release_version[1:]  # Convert to "9.0.5" format

    # Determine the correct `rel-x.x` branch
    target_branch = get_target_release_branch(release_version)
    
    # Retrieve `common.props` data from the target branch
    version, leptonx_version = get_version_from_common_props(target_branch)

    # Skip if the version is a preview or release candidate
    if "preview" in version or "rc" in version:
        return False

    # Read the `latest-versions.json` file
    with open("latest-versions.json", "r") as f:
        latest_versions = json.load(f)

    # Add the new version entry
    new_version_entry = {
        "version": version,
        "releaseDate": "",
        "type": "stable",
        "message": "",
        "leptonx": {
            "version": leptonx_version
        }
    }
    
    latest_versions.insert(0, new_version_entry)  # Insert the new version at the top

    # Update the file
    with open("latest-versions.json", "w") as f:
        json.dump(latest_versions, f, indent=2)

    return True
