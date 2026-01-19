# ABP AI Rules

This folder contains AI rules (Cursor `.mdc` format) for ABP based solutions. These rules help AI assistants understand ABP-specific patterns, conventions, and best practices when working with ABP-based applications.

## Purpose

This folder serves as a central repository for ABP-specific AI rules. The community can contribute, improve, and maintain these rules collaboratively.

When you create a new ABP solution, these rules are included in your project based on your configuration. This provides AI assistants with ABP-specific context, helping them generate code that follows ABP conventions.

> **Important**: These rules are ABP-specific. They don't cover general .NET or ASP.NET Core patterns—AI assistants already know those. Instead, they focus on ABP's unique architecture, module system, and conventions.

## How Rules Work

Large language models don't retain memory between completions. Rules provide persistent, reusable context at the prompt level.

When applied, rule contents are included at the start of the model context. This gives the AI consistent guidance for generating code, interpreting edits, or helping with workflows.

## File Structure

Rules are markdown files with `.mdc` extension. Each rule uses frontmatter metadata to control when it's applied:

```
ai-rules/
├── README.md
├── common/                    # Rules for all templates
│   └── module-architecture.mdc
├── app/                       # Layered app template rules
│   ├── domain-layer.mdc
│   ├── application-layer.mdc
│   └── infrastructure-layer.mdc
└── module/                    # Module template rules
    └── module-development.mdc
```

### Rule Format

Each rule is a markdown file with frontmatter metadata:

```markdown
---
description: "Describes when this rule should apply - used by AI to decide relevance"
globs: "src/**/*.cs"
alwaysApply: false
---

# Rule Title

Your rule content here...
```

### Frontmatter Properties

| Property | Description |
|----------|-------------|
| `description` | Brief description of what the rule covers. Used by AI to determine relevance. |
| `globs` | File patterns that trigger this rule (e.g., `src/**/*.cs`, `*.Domain/**`). |
| `alwaysApply` | If `true`, rule is always included. If `false`, AI decides based on context. |

### Rule Types

| Type | When Applied |
|------|--------------|
| **Always Apply** | Every chat session (`alwaysApply: true`) |
| **Apply Intelligently** | When AI decides it's relevant based on `description` |
| **Apply to Specific Files** | When file matches `globs` pattern |
| **Apply Manually** | When @-mentioned in chat (e.g., `@my-rule`) |

## Best Practices

Good rules are focused, actionable, and scoped:

- **Keep rules under 500 lines** - Split large rules into multiple, composable rules
- **Provide concrete examples** - Reference actual files or include code snippets
- **Be specific, not vague** - Write rules like clear internal documentation
- **Reference files instead of copying** - This keeps rules short and prevents staleness
- **Start simple** - Add rules only when you notice AI making the same mistake repeatedly

## What to Avoid

- **Copying entire style guides**: Use a linter instead. AI already knows common style conventions.
- **Documenting every possible command**: AI knows common tools like `dotnet`, and `npm`.
- **Adding instructions for edge cases that rarely apply**: Keep rules focused on patterns you use frequently.
- **Duplicating what's already in your codebase**: Point to canonical examples instead of copying code.
- **Including non-ABP patterns**: Don't add generic .NET/ASP.NET Core guidance—focus on ABP-specific conventions.

## Contributing

We welcome community contributions to improve these rules! You can open a PR to add new rules or improve existing ones.

Please review our [Contribution Guide](../CONTRIBUTING.md) and [Code of Conduct](../CODE_OF_CONDUCT.md) before contributing.

### Contribution Guidelines

- Each rule should focus on a single ABP concept or pattern
- Use clear, actionable language
- Include examples where helpful
- Test your rules by using them in a real ABP project
- Keep ABP-specific focus—don't add general .NET patterns

## Related Resources

- [Cursor Rules Documentation](https://cursor.com/docs/context/rules)
- [ABP Framework Documentation](https://abp.io/docs)
-