using System.Collections.Generic;

namespace Volo.Abp.Cli.Commands.Models;

public class McpToolDefinition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public McpToolInputSchema InputSchema { get; set; }
}

public class McpToolInputSchema
{
    public Dictionary<string, McpToolProperty> Properties { get; set; }
    public List<string> Required { get; set; }
}

public class McpToolProperty
{
    public string Type { get; set; }
    public string Description { get; set; }
}

