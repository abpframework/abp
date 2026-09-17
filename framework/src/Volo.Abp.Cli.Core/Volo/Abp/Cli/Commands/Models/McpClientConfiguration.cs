using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Volo.Abp.Cli.Commands.Models;

public class McpClientConfiguration
{
    [JsonPropertyName("mcpServers")]
    public Dictionary<string, McpServerConfig> McpServers { get; set; } = new();
}

public class McpServerConfig
{
    [JsonPropertyName("command")]
    public string Command { get; set; }

    [JsonPropertyName("args")]
    public List<string> Args { get; set; } = new();

    [JsonPropertyName("env")]
    public Dictionary<string, string> Env { get; set; }
}

