```json
//[doc-seo]
{
    "Description": "Learn how to configure AI-powered development tools for ABP Framework Angular applications with automatic setup for Claude, Cursor, Copilot, Gemini, Junie, and Windsurf."
}
```

# AI Configuration

ABP Framework provides an **AI Configuration Generator** that helps developers set up AI-powered coding assistants for their Angular applications. This schematic automatically generates configuration files for popular AI tools with pre-configured ABP best practices and guidelines.

## Overview

The AI Configuration Generator is an Angular schematic that creates standardized configuration files for various AI development tools. These configurations include:

- ABP Framework coding standards and best practices
- Angular development guidelines
- Project-specific rules and conventions
- Full-stack development patterns (ABP .NET + Angular)

## Supported AI Tools

The generator supports the following AI coding assistants:

- **Claude** - Creates `.claude/CLAUDE.md` configuration file
- **Copilot** - Creates `.github/copilot-instructions.md` configuration file  
- **Cursor** - Creates `.cursor/rules/cursor.mdc` configuration file
- **Gemini** - Creates `.gemini/GEMINI.md` configuration file
- **Junie** - Creates `.junie/guidelines.md` configuration file
- **Windsurf** - Creates `.windsurf/rules/guidelines.md` configuration file

## Usage

### Basic Usage

Generate AI configuration for a single tool:

```bash
ng g @abp/ng.schematics:ai-config --tool=claude
```

### Multiple Tools

Generate configurations for multiple AI tools at once:

```bash
# Comma-separated
ng g @abp/ng.schematics:ai-config --tool=claude,cursor,copilot

# Space-separated (with quotes)
ng g @abp/ng.schematics:ai-config --tool="claude cursor gemini"

# Multiple --tool flags
ng g @abp/ng.schematics:ai-config --tool=claude --tool=cursor --tool=gemini
```

### Target Specific Project

By default, configurations are generated at the workspace root. To target a specific project:

```bash
ng g @abp/ng.schematics:ai-config --tool=claude --target-project=my-app
```

This creates the configuration files in the `my-app` project root directory.

### Overwrite Existing Files

If configuration files already exist, use the `--overwrite` flag to replace them:

```bash
ng g @abp/ng.schematics:ai-config --tool=cursor --overwrite
```

## Schema Options

The AI Configuration Generator accepts the following options:

### tool

- **Type:** `string`
- **Required:** Yes
- **Description:** Comma-separated list of AI tools to generate configurations for
- **Valid values:** `claude`, `copilot`, `cursor`, `gemini`, `junie`, `windsurf`
- **Example:** `"claude,cursor,copilot"`

### targetProject

- **Type:** `string`
- **Required:** No
- **Description:** The name of the target project in your workspace
- **Default:** Workspace root (`/`)
- **Example:** `"my-angular-app"`

### overwrite

- **Type:** `boolean`
- **Required:** No
- **Default:** `false`
- **Description:** Whether to overwrite existing configuration files

## Configuration Content

All generated configuration files include comprehensive guidelines for:

### General Principles
- Clear separation between backend (ABP/.NET) and frontend (Angular) layers
- Modular architecture patterns
- Official ABP documentation references
- Readability, maintainability, and performance standards

### ABP / .NET Development Rules
- Standard folder structure (`*.Application`, `*.Domain`, `*.EntityFrameworkCore`, `*.HttpApi`)
- C# coding conventions and naming patterns
- Modern C# features (records, pattern matching, null-coalescing)
- ABP module integration (Permissions, Settings, Audit Logging)
- Error handling and validation patterns

### Angular Development Rules
- Angular coding style and best practices
- Component architecture patterns
- Reactive programming with RxJS
- ABP Angular package usage (`@abp/ng.core`, `@abp/ng.theme.shared`)
- State management and service patterns

### Performance and Testing
- Performance optimization techniques
- Unit testing and integration testing guidelines
- Best practices for both backend and frontend

## Examples

### Example 1: Setup Claude for Development

```bash
ng g @abp/ng.schematics:ai-config --tool=claude
```

Output:
```
🚀 Generating AI configuration files...
📁 Target path: /
🤖 Selected tools: claude
✅ AI configuration files generated successfully!

📝 Generated files:
   - .claude/CLAUDE.md

💡 Tip: Restart your IDE or AI tool to apply the new configurations.
```

### Example 2: Setup Multiple Tools for a Project

```bash
ng g @abp/ng.schematics:ai-config --tool="cursor,copilot,gemini" --target-project=acme-app
```

Output:
```
🚀 Generating AI configuration files...
📁 Target path: /acme-app
🤖 Selected tools: cursor, copilot, gemini
✅ AI configuration files generated successfully!

📝 Generated files:
   - /acme-app/.cursor/rules/cursor.mdc
   - /acme-app/.github/copilot-instructions.md
   - /acme-app/.gemini/GEMINI.md

💡 Tip: Restart your IDE or AI tool to apply the new configurations.
```

### Example 3: Update Existing Configuration

```bash
ng g @abp/ng.schematics:ai-config --tool=windsurf --overwrite
```

This will regenerate the Windsurf configuration file even if it already exists.

## File Structure

After running the generator, your project will have configuration files in their respective directories:

```
your-project/
├── .claude/
│   └── CLAUDE.md                    # Claude AI configuration
├── .cursor/
│   └── rules/
│       └── cursor.mdc              # Cursor AI configuration
├── .github/
│   └── copilot-instructions.md     # GitHub Copilot configuration
├── .gemini/
│   └── GEMINI.md                   # Gemini AI configuration
├── .junie/
│   └── guidelines.md               # Junie AI configuration
└── .windsurf/
    └── rules/
        └── guidelines.md           # Windsurf AI configuration
```

## Best Practices

1. **Generate Early**: Set up AI configurations at the beginning of your project to ensure consistent code quality from the start.

2. **Multiple Tools**: If your team uses different AI assistants, generate configurations for all of them to maintain consistency across the team.

3. **Version Control**: Commit the generated configuration files to your repository so all team members benefit from the same AI guidelines.

4. **Keep Updated**: When ABP releases new best practices or your project evolves, regenerate configurations with the `--overwrite` flag.

5. **Project-Specific**: For monorepos or multi-project workspaces, use `--target-project` to create project-specific configurations.

## Troubleshooting

### Configuration File Already Exists

If you see a warning that a configuration file already exists:

```
⚠️  Configuration file already exists: .claude/CLAUDE.md
   Use --overwrite flag to replace existing files.
```

Add the `--overwrite` flag to replace it:

```bash
ng g @abp/ng.schematics:ai-config --tool=claude --overwrite
```

### Invalid Tool Name

If you specify an invalid tool name:

```
Invalid AI tool(s): chatgpt. Valid options are: claude, copilot, cursor, gemini, junie, windsurf
```

Make sure to use only the supported tool names listed above.

### No Tools Selected

If you run the command without specifying any tools:

```bash
ng g @abp/ng.schematics:ai-config
```

You'll see usage examples and available tools:

```
ℹ️  No AI tools selected. Skipping configuration generation.

💡 Usage examples:
   ng g @abp/ng.schematics:ai-config --tool=claude,cursor
   ng g @abp/ng.schematics:ai-config --tool="claude, cursor"
   ng g @abp/ng.schematics:ai-config --tool=gemini --tool=cursor
   ng g @abp/ng.schematics:ai-config --tool=gemini --target-project=my-app

Available tools: claude, copilot, cursor, gemini, junie, windsurf
```