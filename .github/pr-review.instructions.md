# Pull Request Review – GitHub Copilot Instructions

> **Scope**: Pull request reviews in this repository.
>
> **Goal**: Avoid generating noise on machine-generated or automatically merged pull requests, and focus reviews on human-authored changes that require design or code feedback.

---

## Review Scope Filtering

- When reviewing pull requests, **skip and ignore any pull request whose source branch name starts with `auto-merge/`**.
- Do **not** generate review comments, suggestions, summaries, or approvals for these pull requests.
- Treat these pull requests as machine-generated and out of scope for AI review.

## Review Expectations (Applicable PRs Only)

- Only review pull requests created from human-authored feature, bugfix, or maintenance branches.
- Provide feedback that focuses on correctness, design quality, maintainability, and alignment with existing repository conventions.
