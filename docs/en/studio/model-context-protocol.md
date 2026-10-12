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

You can run `abp help mcp-studio` at any time to see the available options and the Cursor and Claude Desktop configuration snippets directly in your terminal.

### Generating Config Files from ABP Studio

By default, the `app`, `app-nolayers` and `microservice` templates create a `.vscode/mcp.json` file, so a solution created from one of them works with VS Code without any extra configuration.

> The endpoint is not authenticated. It only listens on `localhost`, but any local process can call it, and a connected client can read the monitoring data, build the solution, run the configured tasks and start or stop applications and containers. Only configure MCP clients you trust, and do not forward or expose the endpoint.

## Available Tools

ABP Studio exposes the following tools to MCP clients. The monitoring tools work on their own. The build and solution tools need a solution to be open. The application, container and task tools also need a run profile to be selected.

### Monitoring

| Tool | Description |
|------|-------------|
| `get_exceptions` | Gets recent exceptions including stack traces and error messages. |
| `get_logs` | Gets log entries. Can be filtered by log level. |
| `get_requests` | Gets HTTP request information. Can be filtered by status code. |
| `get_events` | Gets distributed events for debugging inter-service communication. |

### Application Control

| Tool | Description |
|------|-------------|
| `start_applications` | Starts applications of the selected run profile. A running application is stopped and started again. |
| `stop_applications` | Stops running applications. |

### Container Control

| Tool | Description |
|------|-------------|
| `start_containers` | Starts Docker containers of the selected run profile. |
| `stop_containers` | Stops Docker containers of the selected run profile. |

### Tasks

| Tool | Description |
|------|-------------|
| `run_task` | Runs a task of the selected run profile. It waits for the task, and on timeout it returns while the task keeps running in the background. |

### Build

| Tool | Description |
|------|-------------|
| `dotnet_build` | Builds the solution, a module or a package using `dotnet build`. |
| `install_libs` | Runs `abp install-libs` at the solution root. |

### Solution Structure

| Tool | Description |
|------|-------------|
| `get_solution_info` | Gets solution name, path, template, and run profile information. |
| `list_modules` | Lists all modules in the solution. |
| `list_packages` | Lists packages (projects) in the solution. Can be filtered by module. |

## Notes

- Monitor data (exceptions, logs, requests, events) is kept in memory and is cleared when the solution is closed.
- The `abp mcp-studio` command connects to the local ABP Studio instance. This is separate from the `abp mcp` command, which connects to the ABP.IO cloud MCP service and requires an active license.
