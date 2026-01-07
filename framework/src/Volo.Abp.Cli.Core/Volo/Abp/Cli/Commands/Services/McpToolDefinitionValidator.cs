using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands.Models;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class McpToolDefinitionValidator : ITransientDependency
{
    private const int MaxToolNameLength = 100;
    private const int MaxDescriptionLength = 2000;
    private const string UnknownToolName = "<unknown>";
    
    private static readonly Regex ToolNameRegex = new Regex("^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
    private static readonly HashSet<string> ValidTypeValues = new HashSet<string> 
        { "string", "number", "boolean", "object", "array" };

    private readonly ILogger<McpToolDefinitionValidator> _logger;

    public McpToolDefinitionValidator(ILogger<McpToolDefinitionValidator> logger)
    {
        _logger = logger;
    }

    public List<McpToolDefinition> ValidateAndFilter(List<McpToolDefinition> tools)
    {
        if (tools == null || tools.Count == 0)
        {
            return new List<McpToolDefinition>();
        }

        var validTools = new List<McpToolDefinition>();

        foreach (var tool in tools)
        {
            try
            {
                if (!IsValidTool(tool))
                {
                    continue;
                }

                validTools.Add(tool);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error validating tool '{tool?.Name ?? UnknownToolName}': {ex.Message}");
            }
        }

        if (validTools.Count < tools.Count)
        {
            _logger.LogWarning($"Filtered out {tools.Count - validTools.Count} invalid tool(s). {validTools.Count} valid tool(s) remaining.");
        }

        return validTools;
    }

    private bool IsValidTool(McpToolDefinition tool)
    {
        if (!IsValidToolName(tool))
        {
            return false;
        }

        if (!IsValidDescription(tool))
        {
            return false;
        }

        if (tool.InputSchema != null && !IsValidInputSchema(tool))
        {
            return false;
        }

        return true;
    }

    private bool IsValidToolName(McpToolDefinition tool)
    {
        if (string.IsNullOrWhiteSpace(tool.Name))
        {
            _logger.LogWarning($"Skipping tool with empty name");
            return false;
        }

        if (tool.Name.Length > MaxToolNameLength)
        {
            _logger.LogWarning($"Skipping tool '{tool.Name}' with name exceeding {MaxToolNameLength} characters");
            return false;
        }

        if (!ToolNameRegex.IsMatch(tool.Name))
        {
            _logger.LogWarning($"Skipping tool with invalid name format: {tool.Name}");
            return false;
        }

        return true;
    }

    private bool IsValidDescription(McpToolDefinition tool)
    {
        if (string.IsNullOrWhiteSpace(tool.Description))
        {
            _logger.LogWarning($"Skipping tool '{tool.Name}' with empty description");
            return false;
        }

        if (tool.Description.Length > MaxDescriptionLength)
        {
            _logger.LogWarning($"Skipping tool '{tool.Name}' with description exceeding {MaxDescriptionLength} characters");
            return false;
        }

        return true;
    }

    private bool IsValidInputSchema(McpToolDefinition tool)
    {
        if (!ArePropertiesValid(tool))
        {
            return false;
        }

        if (!AreRequiredFieldsValid(tool))
        {
            return false;
        }

        return true;
    }

    private bool ArePropertiesValid(McpToolDefinition tool)
    {
        if (tool.InputSchema.Properties == null)
        {
            return true;
        }

        foreach (var property in tool.InputSchema.Properties)
        {
            if (string.IsNullOrWhiteSpace(property.Value?.Type) ||
                !ValidTypeValues.Contains(property.Value.Type))
            {
                _logger.LogWarning($"Skipping tool '{tool.Name}' with invalid property type: {property.Value?.Type ?? UnknownToolName}");
                return false;
            }

            if (property.Value.Description != null && property.Value.Description.Length > MaxDescriptionLength)
            {
                _logger.LogWarning($"Skipping tool '{tool.Name}' with property description exceeding {MaxDescriptionLength} characters");
                return false;
            }
        }

        return true;
    }

    private bool AreRequiredFieldsValid(McpToolDefinition tool)
    {
        if (tool.InputSchema.Required == null || tool.InputSchema.Properties == null)
        {
            return true;
        }

        foreach (var required in tool.InputSchema.Required)
        {
            if (!tool.InputSchema.Properties.ContainsKey(required))
            {
                _logger.LogWarning($"Skipping tool '{tool.Name}' with required field '{required}' not in properties");
                return false;
            }
        }

        return true;
    }
}

