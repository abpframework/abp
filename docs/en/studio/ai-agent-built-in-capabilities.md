```json
//[doc-seo]
{
    "Description": "Capability matrix for ABP Studio AI Agent modes, built-in tools, Studio automation tools, connected MCP tools, and subagents."
}
```

# ABP Studio: AI Agent Built-in Capabilities

````json
//[doc-nav]
{
  "Next": {
    "Name": "AI Agent Git Integration",
    "Path": "studio/ai-agent-git-integration"
  }
}
````

ABP Studio exposes tools to the AI Agent based on the selected mode, run context, tool settings, permissions, and connected MCP server tools. The tool list is locked per running Agent-mode session to keep prompt caching and execution stable while other sessions run in parallel.

## Mode Capability Matrix

| Capability | Agent | Plan | Ask |
| --- | --- | --- | --- |
| Find files | Yes | Yes | Yes |
| Read files | Yes | Yes | Yes |
| Search code | Yes | Yes | Yes |
| ABP-aware outlines | Yes | Yes | Yes |
| Semantic code search | Yes | Yes | Yes |
| Ask user | Yes | Yes | Yes |
| Web search | Yes | Yes | Yes |
| Fetch URL | Yes | Yes | Yes |
| Fetch AI skills | Yes, when available | Yes, when available | Yes, when available |
| Write/edit/delete/rename files | Yes | No | No |
| Download files | Yes | No | No |
| Save learned lessons | Yes | No | No |
| Run shell command | Yes | No | No |
| Add migration | Yes | No | No |
| Create/read/update plan files | No | Yes | No |
| Update active plan steps | Yes | No | No |
| Search/read ABP docs directly | No | No | Yes |
| Studio automation tools | Yes | No | No |
| Connected MCP tools | Yes | No | No |
| Spawn subagents | Yes | Yes | Yes |

ABP documentation tools are available to Ask mode and ABP documentation subagents. The main Agent-mode tool list does not include direct ABP docs search/read tools; Agent mode can use the ABP documentation subagent when documentation research is needed.

## File Tools

The file tools are scope-aware and `.abpignore`-aware.

| Tool | Description |
| --- | --- |
| `find_files` | Finds files by filename or glob under accessible directories. Build output and dependency folders such as `bin`, `obj`, and `node_modules` are excluded. |
| `read_files` | Reads up to 10 files per call. Supports line ranges. Large files are automatically truncated. |
| `write_file` | Writes file content in Agent mode. |
| `edit_file_batch` | Applies structured edits in Agent mode. |
| `delete_path` | Deletes a file or directory in Agent mode. |
| `rename_path` | Renames or moves a file or directory in Agent mode. |

Write operations use file change tracking. When another session modifies a file after it was read, the agent is instructed to re-read before overwriting.

## Code Intelligence

| Tool | Description |
| --- | --- |
| `search_code` | Searches exact or regex text across source and configuration files. |
| `get_outlines` | Returns ABP-aware outlines for code structures such as entities, DTOs, services, repositories, DbContexts, controllers, modules, and methods. |
| `semantic_code_search` | Searches indexed code by meaning rather than exact text. |

These tools are available in all modes and are the primary way the agent grounds answers in the current solution.

## Web And External Content

| Tool | Description |
| --- | --- |
| `web_search` | Searches the web for current or external information. |
| `fetch_url` | Fetches a URL after domain permission is granted. Large content is cached under `.abpstudio/url-cache`. |
| `download_file` | Downloads a file in Agent mode after explicit permission. Downloads are limited to 10 MB. |

URL fetch and download permissions are independent. Downloads always require explicit confirmation.

## User Interaction

The `ask_user` tool lets the agent request missing information during a run. Pending questions are tracked per session, so parallel sessions can wait for independent user input without sharing state.

## Shell And Migration Tools

Agent mode can run shell commands through `run_shell_command`. Shell execution is permission-gated.

The `add_migration` tool adds Entity Framework Core migrations for configured migration projects.

## Studio Automation Tools

Agent mode can use enabled ABP Studio automation tools.

| Tool | Description |
| --- | --- |
| `dotnet_build` | Builds the solution or targeted project context. |
| `build_solution` | Builds the whole solution when exposed by the run context. |
| `build_module` | Builds a selected module. |
| `build_package` | Builds a selected package. |
| `get_exceptions` | Reads monitored application exceptions. |
| `get_logs` | Reads monitored application logs. |
| `get_requests` | Reads monitored HTTP request data. |
| `get_events` | Reads monitored distributed event data. |
| `start_applications` | Starts applications from the active run profile. |
| `stop_applications` | Stops applications from the active run profile. |
| `start_containers` | Starts containers from the active run profile. |
| `stop_containers` | Stops containers from the active run profile. |
| `run_task` | Runs configured Studio tasks. |
| `install_libs` | Installs client-side library files. |
| `generate_csharp_proxies` | Generates C# client proxies. |
| `generate_angular_proxies` | Generates Angular client proxies. |

## Subagents

The `spawn_subagent` tool delegates bounded research work to read-only specialist subagents.

| Subagent type | Purpose |
| --- | --- |
| `research` | Performs solution/code research using read-only tools. |
| `websearch` | Performs web-focused research. |
| `abpdocsearcher` | Searches and reads ABP documentation for framework-specific questions. |

Subagents are read-only. They are intended for parallel research and return summarized findings to the main agent.

## MCP Tools

Configured MCP server tools can be added to Agent mode when:

- The MCP server is configured and enabled.
- The server connection is successful.
- The individual MCP tool is not disabled in settings.

MCP resources are visible in settings and can be opened for inspection, but the Agent-mode callable surface is composed from MCP tools. Plan and Ask modes do not receive MCP tools.

This capability lets the AI Agent consume tools from external MCP servers. It does not make ABP Studio AI Agent an MCP server for external clients.
