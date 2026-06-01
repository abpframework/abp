```json
//[doc-seo]
{
    "Description": "Learn how to connect AI tools like Cursor, Claude Desktop, and VS Code to ABP Studio using the Model Context Protocol (MCP), and see the tools currently exposed by Studio."
}
```

# ABP Studio: Model Context Protocol (MCP)

````json
//[doc-nav]
{
  "Next": {
    "Name": "Working with Kubernetes",
    "Path": "studio/kubernetes"
  }
}
````

ABP Studio includes built-in [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) support so AI tools can inspect runtime telemetry, work with the Solution Runner, build projects, generate proxies, run custom commands, inspect Kubernetes-related data, and automate ABP Studio's embedded browser.

## How It Works

ABP Studio runs a local MCP server in the background. The `abp mcp-studio` CLI command acts as a stdio bridge that AI clients connect to. The bridge forwards requests to ABP Studio and returns responses.

```text
MCP Client (Cursor / Claude Desktop / VS Code)
  ──stdio──▶  abp mcp-studio  ──HTTP──▶  ABP Studio
```

> ABP Studio must be running while MCP is used. If ABP Studio is not running (or its MCP endpoint is unavailable), `abp mcp-studio` returns an error to the AI client.

By default, the bridge connects to `http://localhost:38280/mcp/`. You can override this with the `--endpoint` option.

## Configuration

### Cursor (`.cursor/mcp.json`)

```json
{
  "mcpServers": {
    "abp-studio": {
      "command": "abp",
      "args": ["mcp-studio"]
    }
  }
}
```

### Claude Desktop (`claude_desktop_config.json`)

```json
{
  "mcpServers": {
    "abp-studio": {
      "command": "abp",
      "args": ["mcp-studio"]
    }
  }
}
```

Claude Desktop config file locations:

- macOS: `~/Library/Application Support/Claude/claude_desktop_config.json`
- Windows: `%APPDATA%\Claude\claude_desktop_config.json`
- Linux: `~/.config/Claude/claude_desktop_config.json`

### VS Code (`.vscode/mcp.json`)

```json
{
  "servers": {
    "abp-studio": {
      "command": "abp",
      "args": ["mcp-studio"]
    }
  }
}
```

### Quick Reference

You can run `abp help mcp-studio` at any time to see the available options and example configuration snippets directly in your terminal.

### Generating Config Files from ABP Studio

ABP Studio solution templates can generate the VS Code MCP configuration file (`.vscode/mcp.json`) during solution creation. Cursor and Claude Desktop can be configured with the snippets shown above.

## Available Tools

ABP Studio exposes the following tools to MCP clients. All tools operate on the currently open solution. Tools that interact with Solution Runner also require a selected run profile.

### Runtime Monitoring

| Tool | Description |
|------|-------------|
| `get_exceptions` | Gets recent exceptions including stack traces and error messages. |
| `get_logs` | Gets runtime log entries. Can be filtered by application name and minimum log level. |
| `get_requests` | Gets HTTP request information. Can be filtered by application name, status code, and URL substring. |
| `get_events` | Gets distributed events for debugging inter-service communication. |

### Solution Runner

| Tool | Description |
|------|-------------|
| `start_applications` | Starts or restarts one or more applications by name, folder, or the entire application tree. |
| `stop_applications` | Stops one or more running applications by name, folder, or the entire application tree. |
| `start_containers` | Starts Docker containers in the selected run profile. |
| `stop_containers` | Stops Docker containers in the selected run profile. |
| `run_task` | Runs a Solution Runner task and waits for it to finish. |

### Solution Structure

| Tool | Description |
|------|-------------|
| `get_solution_info` | Gets solution name, path, template, module count, and run profile information. |
| `list_modules` | Lists all modules in the solution. |
| `list_packages` | Lists packages (projects) in the solution. Can be filtered by module. |

### Build and Generation

| Tool | Description |
|------|-------------|
| `dotnet_build` | Builds the whole solution, selected modules, or selected packages by using `dotnet build`. |
| `install_libs` | Runs `abp install-libs` at the solution root. |
| `generate_csharp_proxies` | Generates C# static client proxies from a running API application. |
| `generate_angular_proxies` | Generates Angular service proxies from a running API application. |

### Custom Commands and Kubernetes

| Tool | Description |
|------|-------------|
| `list_custom_commands` | Lists custom commands defined in the current solution. |
| `run_custom_command` | Runs a custom command for a supported target and waits for completion. |
| `list_kubernetes_charts` | Lists Helm charts defined in the solution. |
| `list_kubernetes_services` | Lists services from the selected Kubernetes profile. |

### Embedded Browser

| Tool | Description |
|------|-------------|
| `browser_list_tabs` | Lists open tabs in ABP Studio's embedded browser. |
| `browser_open` | Opens or navigates an embedded browser tab to a URL. |
| `browser_snapshot` | Returns the current page title, URL, visible text, and interactive elements. |
| `browser_wait_for` | Waits for time-based or text-based conditions in a browser tab. |
| `browser_screenshot` | Captures a PNG screenshot of the selected browser tab. |
| `browser_click` | Clicks an element in the embedded browser by CSS selector. |
| `browser_type` | Types into an editable element in the embedded browser by CSS selector. |
| `browser_evaluate` | Runs JavaScript in the selected embedded browser tab and returns the result. |
| `browser_console` | Reads captured console output from the selected embedded browser tab. |

## Notes

- Monitor data (exceptions, logs, requests, events) is kept in memory, capped at 100 entries per application for each data type, and is cleared when the solution is closed.
- A dedicated `clear_monitor` MCP tool is not currently exposed. Closing the solution in ABP Studio clears the collected monitor data.
- Some tools depend on the current Studio context. For example, Solution Runner tools need a selected run profile, and `list_kubernetes_services` uses the selected Kubernetes profile.
- The `abp mcp-studio` command connects to the local ABP Studio instance. This is separate from the `abp mcp` command, which connects to the ABP.IO cloud MCP service and requires an active license.
