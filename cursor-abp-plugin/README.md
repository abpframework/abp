# ABP Plugin for Cursor

ABP development toolkit for Cursor — framework rules, best practices, and ABP Studio integration via MCP.

## What's Included

### Rules

This plugin bundles **16 curated rules** that teach Cursor how to write idiomatic ABP Framework code. Rules activate automatically based on the files you're working with.

| Rule | Description |
|------|-------------|
| **abp-core** | Core ABP Framework conventions — module system, dependency injection, and base classes |
| **application-layer** | Application Services, DTOs, validation, and error handling patterns |
| **authorization** | Permission system and authorization patterns |
| **cli-commands** | ABP CLI commands: `generate-proxy`, `install-libs`, `add-package-ref`, `new-module`, `install-module`, `update`, `clean`, `suite generate` |
| **ddd-patterns** | DDD patterns — Entities, Aggregate Roots, Repositories, Domain Services |
| **dependency-rules** | Layer dependency rules and project structure guardrails |
| **development-flow** | Development workflow — adding features, entities, and migrations |
| **ef-core** | Entity Framework Core patterns — DbContext, migrations, repositories |
| **infrastructure** | Infrastructure services — Settings, Features, Caching, Events, Background Jobs |
| **mongodb** | MongoDB patterns — MongoDbContext and repositories |
| **multi-tenancy** | Multi-Tenancy patterns — tenant-aware entities, data isolation, and tenant switching |
| **solution-templates** | Solution template structures — detect and follow the correct template patterns (app, app-nolayers, module, microservice) |
| **testing-patterns** | Testing patterns — unit tests and integration tests |
| **ui-angular** | Angular UI patterns and best practices |
| **ui-blazor** | Blazor UI patterns and components |
| **ui-mvc** | MVC and Razor Pages UI patterns |

### ABP Studio MCP Server

This plugin includes an [MCP](https://modelcontextprotocol.io/) server configuration that connects Cursor to your local ABP Studio instance. ABP Studio exposes runtime telemetry and solution control tools through the Model Context Protocol. See the [ABP Studio MCP documentation](https://abp.io/docs/latest/studio/model-context-protocol) for more details.

**Prerequisites:** ABP Studio must be running while MCP is in use. The `abp` CLI must be installed and available in your PATH. If you have the ABP Studio desktop application installed, the CLI is already installed and added to your PATH by default.

#### Available MCP Tools

**Monitoring**

| Tool | Description |
|------|-------------|
| `list_applications` | Lists all running ABP applications connected to ABP Studio |
| `get_exceptions` | Gets recent exceptions including stack traces and error messages |
| `get_logs` | Gets log entries (can be filtered by log level) |
| `get_requests` | Gets HTTP request information (can be filtered by status code) |
| `get_events` | Gets distributed events for debugging inter-service communication |
| `clear_monitor` | Clears collected monitor data |

**Application Control**

| Tool | Description |
|------|-------------|
| `list_runnable_applications` | Lists all applications in the current run profile with their state |
| `start_application` | Starts a stopped application |
| `stop_application` | Stops a running application |
| `restart_application` | Restarts a running application |
| `build_application` | Builds a .NET application using `dotnet build` |

**Container Control**

| Tool | Description |
|------|-------------|
| `list_containers` | Lists Docker containers in the current run profile with their state |
| `start_containers` | Starts Docker containers (docker-compose up) |
| `stop_containers` | Stops Docker containers (docker-compose down) |

**Solution Structure**

| Tool | Description |
|------|-------------|
| `get_solution_info` | Gets solution name, path, template, and run profile information |
| `list_modules` | Lists all modules in the solution |
| `list_packages` | Lists packages (projects) in the solution (can be filtered by module) |
| `get_module_dependencies` | Gets module dependency/import information |

## Usage

After installing the plugin in Cursor:

- **Rules** activate automatically when writing, reviewing, or refactoring ABP Framework code. They provide guidance on DDD patterns, module architecture, authorization, UI layers, testing, and more.
- **ABP Studio MCP tools** let you interact with your running applications directly from Cursor:
  - "Show me the recent exceptions from my application"
  - "List all running applications"
  - "Restart the Web application"
  - "Get the latest logs with Warning level"
  - "What modules are in this solution?"

## How the MCP Server Works

ABP Studio runs a local MCP server in the background. The `abp mcp-studio` CLI command acts as a stdio bridge that Cursor connects to. The bridge forwards requests to ABP Studio and returns responses.

```
Cursor  ──stdio──▶  abp mcp-studio  ──HTTP──▶  ABP Studio
```

> Monitor data (exceptions, logs, requests, events) is kept in memory and is cleared when the solution is closed.

## Directory Structure

```
cursor-abp-plugin/
├── .cursor-plugin/
│   └── plugin.json          # Plugin manifest
├── .mcp.json                # MCP server configuration
├── assets/
│   └── abp-icon.svg         # Plugin logo
├── rules/                   # All rules as .mdc files
│   ├── abp-core.mdc
│   ├── application-layer.mdc
│   ├── authorization.mdc
│   ├── cli-commands.mdc
│   ├── ddd-patterns.mdc
│   ├── dependency-rules.mdc
│   ├── development-flow.mdc
│   ├── ef-core.mdc
│   ├── infrastructure.mdc
│   ├── mongodb.mdc
│   ├── multi-tenancy.mdc
│   ├── solution-templates.mdc
│   ├── testing-patterns.mdc
│   ├── ui-angular.mdc
│   ├── ui-blazor.mdc
│   └── ui-mvc.mdc
└── README.md                # This file
```

## Credits

- **Rules** by [Volosoft](https://volosoft.com)
- **ABP Studio MCP Server** by [Volosoft](https://volosoft.com)
- **Plugin Specification** by [Cursor](https://cursor.com)
