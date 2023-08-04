import os
import json
from github import Github

def update_latest_versions():
    version = os.environ["GITHUB_REF"].split("/")[-1]

    if "rc" in version:
        return False

    with open("latest-versions.json", "r") as f:
        latest_versions = json.load(f)

    latest_versions[0]["version"] = version

    with open("latest-versions.json", "w") as f:
        json.dump(latest_versions, f, indent=2)

    return True

def create_pr():
    g = Github(os.environ["GITHUB_TOKEN"])
    repo = g.get_repo("abpframework/abp")

    branch_name = f"update-latest-versions-{os.environ['GITHUB_REF'].split('/')[-1]}"
    base = repo.get_branch("dev")
    repo.create_git_ref(ref=f"refs/heads/{branch_name}", sha=base.commit.sha)

    # Get the current latest-versions.json file and its sha
    contents = repo.get_contents("latest-versions.json", ref="dev")
    file_sha = contents.sha

    # Update the file in the repo
    repo.update_file(
        path="latest-versions.json",
        message=f"Update latest-versions.json to version {os.environ['GITHUB_REF'].split('/')[-1]}",
        content=open("latest-versions.json", "r").read().encode("utf-8"),
        sha=file_sha,
        branch=branch_name,
    )

    try:
        pr = repo.create_pull(title="Update latest-versions.json",
                        body="Automated PR to update the latest-versions.json file.",
                        head=branch_name, base="dev")
    except Exception as e:
        print(f"Error while creating PR: {e}")

    pr.create_review_request(reviewers=["ebicoglu", "gizemmutukurt", "skoc10"])

if __name__ == "__main__":
    should_create_pr = update_latest_versions()
    if should_create_pr:
        create_pr()