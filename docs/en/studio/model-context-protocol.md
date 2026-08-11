```json
//[doc-seo]
{
    "Description": "Learn how to connect AI tools like Cursor, Claude Desktop, and VS Code to ABP Studio using the Model Context Protocol (MCP)."
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

ABP Studio includes built-in [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) support so AI tools can query runtime telemetry and control solution runner operations.

## How It Works

ABP Studio runs a local MCP server in the background. The `abp mcp-studio` CLI command acts as a stdio bridge that AI clients connect to. The bridge forwards requests to ABP Studio and returns responses.

```text
MCP Client (Cursor / Claude Desktop / VS Code)
  ──stdio──▶  abp mcp-studio  ──HTTP──▶  ABP Studio
```

> ABP Studio must be running while MCP is used. If ABP Studio is not running (or its MCP endpoint is unavailable), `abp mcp-studio` returns an error to the AI client.

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

You can run `abp help mcp-studio` at any time to see the available options and example configuration snippets for each supported IDE directly in your terminal.

### Generating Config Files from ABP Studio

When creating a new solution, ABP Studio can generate MCP configuration files for Cursor and VS Code automatically.

## Available Tools

ABP Studio exposes the following tools to MCP clients. All tools operate on the currently open solution and selected run profile in ABP Studio.

### Monitoring

| Tool | Description |
|------|-------------|
| `list_applications` | Lists all running ABP applications connected to ABP Studio. |
| `get_exceptions` | Gets recent exceptions including stack traces and error messages. |
| `get_logs` | Gets log entries. Can be filtered by log level. |
| `get_requests` | Gets HTTP request information. Can be filtered by status code. |
| `get_events` | Gets distributed events for debugging inter-service communication. |
| `clear_monitor` | Clears collected monitor data. |

### Application Control

| Tool | Description |
|------|-------------|
| `list_runnable_applications` | Lists all applications in the current run profile with their state. |
| `start_application` | Starts a stopped application. |
| `stop_application` | Stops a running application. |
| `restart_application` | Restarts a running application. |
| `build_application` | Builds a .NET application using `dotnet build`. |

### Container Control

| Tool | Description |
|------|-------------|
| `list_containers` | Lists Docker containers in the current run profile with their state. |
| `start_containers` | Starts Docker containers (docker-compose up). |
| `stop_containers` | Stops Docker containers (docker-compose down). |

### Solution Structure

| Tool | Description |
|------|-------------|
| `get_solution_info` | Gets solution name, path, template, and run profile information. |
| `list_modules` | Lists all modules in the solution. |
| `list_packages` | Lists packages (projects) in the solution. Can be filtered by module. |
| `get_module_dependencies` | Gets module dependency/import information. |

## Notes

- Monitor data (exceptions, logs, requests, events) is kept in memory and is cleared when the solution is closed.
- The `abp mcp-studio` command connects to the local ABP Studio instance. This is separate from the `abp mcp` command, which connects to the ABP.IO cloud MCP service and requires an active license.
