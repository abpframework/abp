```json
//[doc-seo]
{
    "Description": "Technical guidance for coding with ABP Studio AI Agent, including mode selection, scopes, prompting, verification, Git review handoff, Studio tools, MCP tools, and parallel sessions."
}
```

# ABP Studio: Coding with AI Agent

This document describes coding practices for ABP Studio AI Agent. It focuses on how to shape agent runs so the agent receives the correct context, uses the correct tools, and verifies changes with the same ABP Studio automation available to the developer.

## Mode Selection

Use the mode that matches the expected side effect.

| Work type | Mode |
| --- | --- |
| Code changes, migrations, builds, proxy generation, app/container control, Studio tool execution, or MCP tool execution | Agent |
| Technical implementation design before coding | Plan |
| Explanation, source navigation, ABP documentation lookup, and codebase questions | Ask |

Plan and Ask are read-only modes. They are appropriate when the desired output is a technical answer or implementation plan rather than modified files.

## Scope Selection

Set the AI scope before the first prompt in a session. The first message locks the scope for that session.

Use a narrow scope when the task is isolated to one module or package. Use the whole-solution scope when the task depends on cross-module contracts, shared infrastructure, or multiple applications.

External folders should be added only when their contents are required by the task. Files outside the resolved scope are not accessible to agent file tools.

## Prompt Context

Prompts should identify the target behavior, relevant packages, constraints, and expected verification. The agent already receives solution and run profile context, so prompts should focus on task-specific decisions.

Useful prompt context includes:

- The user-facing behavior to change.
- Affected module, package, or application names.
- Relevant file paths when known.
- API, DTO, database, or permission constraints.
- Required validation commands or Studio workflow.
- Whether migrations or proxy generation are expected.

Avoid placing durable coding rules in every prompt. Use AI rules for project standards and prompts for task-specific requirements.

## Changed-File Prompting

When the task is based on current Git changes, use the Git panel selection or review handoff instead of manually describing all changes. Studio can build prompts from selected staged or working-tree diffs, AI suggestions, user review notes, line numbers, and line content.

This keeps the prompt aligned with the actual Git state and avoids missing files that are selected in the commit/review workflow.

## AI Review Handoff

AI Review is a review workflow, not an implementation workflow. After suggestions are produced, send the review content to AI Agent when the intent is to apply fixes.

The handoff prompt contains verified suggestions, user notes, file paths, and line references. The agent then runs in the normal coding workflow and can edit files, run builds, and update active plan steps depending on the session mode and tools.

## Plans

Use Plan mode for changes with broad design impact, uncertain architecture, or multiple implementation phases. Plan files are stored under `.abpstudio/plans` and can be applied later in Agent mode.

An active plan gives Agent mode a durable implementation checklist. Agent mode updates plan step state as work completes.

## Verification

Agent mode has access to Studio build and automation tools when enabled. Verification should match the changed surface.

| Change type | Typical verification |
| --- | --- |
| Application or domain code | Build affected package or solution. |
| Entity or DbContext changes | Add migration if required, then build. |
| HTTP API contract changes | Regenerate C# or Angular proxies when the consuming project requires them. |
| UI or client library changes | Install libs when new ABP module library assets are introduced. |
| Runtime behavior | Start required applications or containers and inspect logs, requests, exceptions, or events. |
| Review fixes | Re-run AI Review or build/test the affected packages. |

Workflow after tasks can encode common verification steps, but the agent can skip workflow actions that do not apply to the completed changes.

## Migrations

Database migrations should be generated only when model changes require them. Workflows can configure the migration project so the agent can add a migration through the `add_migration` tool.

Migration generation is serialized with build operations to avoid concurrent `dotnet` conflicts across sessions.

## Proxy Generation

Proxy generation is appropriate when API contract changes must be consumed by C# or Angular clients.

Workflow actions can configure:

- API base URL.
- Module name.
- Service type filter: `all`, `application`, or `integration`.
- Output folder or Angular working directory.
- C# target package.
- Whether C# contract classes are generated.

## MCP Tools

Use configured MCP tools when the agent must work with an external system that is not covered by built-in ABP Studio tools. MCP tools are available only in Agent mode, only when the MCP server is connected, and only when the individual tool is enabled.

Because MCP tools can have side effects defined by the connected server, keep MCP connections limited to trusted servers and disable tools that should not be callable by the AI Agent.

MCP tool configuration is for adding external tools to the AI Agent. It does not expose ABP Studio AI Agent as an MCP server for other AI clients.

## AI Rules And Lessons

Use always-apply AI rules for standards that should affect every run, such as architecture rules, naming conventions, security requirements, or team-specific ABP patterns.

Use on-demand AI skills for specialized knowledge that should be loaded only when relevant. This keeps the system prompt smaller and avoids applying unrelated instructions.

Learned lessons are for verified corrections. They should represent durable mistakes and corrections, not task notes.

## Parallel Sessions

Parallel sessions are useful for independent work, research, and review. Each session locks its model settings, scope, workflow, and tool snapshot when it starts.

Avoid running multiple Agent-mode sessions that edit the same files. Studio detects some stale-file situations and serializes selected tool operations, but it does not replace normal coordination for overlapping edits.

## Limits And Failure Modes

Common boundaries:

- Files outside the active AI scope are inaccessible.
- Files matched by `.abpignore` are inaccessible.
- Image attachments require a model with image support.
- Shell, URL fetch, and download operations can wait for permission.
- Disabled Studio tools and disabled MCP tools are omitted from the tool list.
- Plan and Ask modes cannot modify files.
- Hidden internal Studio tools are not exposed to the agent.
