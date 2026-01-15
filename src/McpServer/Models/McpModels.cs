using System.Text.Json.Serialization;

namespace McpServer.Models;

public class InitializeParams
{
    [JsonPropertyName("protocolVersion")]
    public string ProtocolVersion { get; set; } = "2024-11-05";

    [JsonPropertyName("capabilities")]
    public ClientCapabilities? Capabilities { get; set; }

    [JsonPropertyName("clientInfo")]
    public Implementation? ClientInfo { get; set; }
}

public class ClientCapabilities
{
    [JsonPropertyName("roots")]
    public RootsCapability? Roots { get; set; }

    [JsonPropertyName("sampling")]
    public object? Sampling { get; set; }
}

public class RootsCapability
{
    [JsonPropertyName("listChanged")]
    public bool? ListChanged { get; set; }
}

public class Implementation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

public class InitializeResult
{
    [JsonPropertyName("protocolVersion")]
    public string ProtocolVersion { get; set; } = "2024-11-05";

    [JsonPropertyName("capabilities")]
    public ServerCapabilities Capabilities { get; set; } = new();

    [JsonPropertyName("serverInfo")]
    public Implementation ServerInfo { get; set; } = new();
}

public class ServerCapabilities
{
    [JsonPropertyName("tools")]
    public object? Tools { get; set; }

    [JsonPropertyName("resources")]
    public object? Resources { get; set; }

    [JsonPropertyName("prompts")]
    public object? Prompts { get; set; }
}

public class Tool
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("inputSchema")]
    public object InputSchema { get; set; } = new { };
}

public class ListToolsResult
{
    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; } = new();
}

public class CallToolParams
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("arguments")]
    public Dictionary<string, object>? Arguments { get; set; }
}

public class CallToolResult
{
    [JsonPropertyName("content")]
    public List<Content> Content { get; set; } = new();
}

public class Content
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class Resource
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }
}

public class ListResourcesResult
{
    [JsonPropertyName("resources")]
    public List<Resource> Resources { get; set; } = new();
}

public class ReadResourceParams
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;
}

public class ReadResourceResult
{
    [JsonPropertyName("contents")]
    public List<ResourceContent> Contents { get; set; } = new();
}

public class ResourceContent
{
    [JsonPropertyName("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class Prompt
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("arguments")]
    public List<PromptArgument>? Arguments { get; set; }
}

public class PromptArgument
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}

public class ListPromptsResult
{
    [JsonPropertyName("prompts")]
    public List<Prompt> Prompts { get; set; } = new();
}

public class GetPromptParams
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("arguments")]
    public Dictionary<string, string>? Arguments { get; set; }
}

public class GetPromptResult
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("messages")]
    public List<PromptMessage> Messages { get; set; } = new();
}

public class PromptMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = "user";

    [JsonPropertyName("content")]
    public Content Content { get; set; } = new();
}
