using System.Text.Json;
using McpServer.Models;

namespace McpServer.Handlers;

public class McpServerHandler
{
    private bool _initialized = false;
    private readonly List<Tool> _tools = new();
    private readonly List<Resource> _resources = new();
    private readonly List<Prompt> _prompts = new();

    public McpServerHandler()
    {
        RegisterDefaultTools();
        RegisterDefaultResources();
        RegisterDefaultPrompts();
    }

    private void RegisterDefaultTools()
    {
        _tools.Add(new Tool
        {
            Name = "calculate",
            Description = "Performs basic arithmetic calculations (add, subtract, multiply, divide)",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    operation = new
                    {
                        type = "string",
                        description = "The operation to perform",
                        @enum = new[] { "add", "subtract", "multiply", "divide" }
                    },
                    a = new
                    {
                        type = "number",
                        description = "First operand"
                    },
                    b = new
                    {
                        type = "number",
                        description = "Second operand"
                    }
                },
                required = new[] { "operation", "a", "b" }
            }
        });

        _tools.Add(new Tool
        {
            Name = "echo",
            Description = "Echoes back the provided message",
            InputSchema = new
            {
                type = "object",
                properties = new
                {
                    message = new
                    {
                        type = "string",
                        description = "The message to echo"
                    }
                },
                required = new[] { "message" }
            }
        });
    }

    private void RegisterDefaultResources()
    {
        _resources.Add(new Resource
        {
            Uri = "example://readme",
            Name = "Server README",
            Description = "Information about this MCP server",
            MimeType = "text/plain"
        });
    }

    private void RegisterDefaultPrompts()
    {
        _prompts.Add(new Prompt
        {
            Name = "greeting",
            Description = "Generate a friendly greeting",
            Arguments = new List<PromptArgument>
            {
                new PromptArgument
                {
                    Name = "name",
                    Description = "Name of the person to greet",
                    Required = true
                }
            }
        });
    }

    public object HandleRequest(JsonRpcRequest request)
    {
        return request.Method switch
        {
            "initialize" => HandleInitialize(request),
            "initialized" => new { }, // Notification, no response needed
            "tools/list" => HandleListTools(),
            "tools/call" => HandleCallTool(request),
            "resources/list" => HandleListResources(),
            "resources/read" => HandleReadResource(request),
            "prompts/list" => HandleListPrompts(),
            "prompts/get" => HandleGetPrompt(request),
            _ => throw new Exception($"Unknown method: {request.Method}")
        };
    }

    private InitializeResult HandleInitialize(JsonRpcRequest request)
    {
        _initialized = true;

        return new InitializeResult
        {
            ProtocolVersion = "2024-11-05",
            ServerInfo = new Implementation
            {
                Name = "McpServer",
                Version = "1.0.0"
            },
            Capabilities = new ServerCapabilities
            {
                Tools = new { },
                Resources = new { },
                Prompts = new { }
            }
        };
    }

    private ListToolsResult HandleListTools()
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        return new ListToolsResult { Tools = _tools };
    }

    private CallToolResult HandleCallTool(JsonRpcRequest request)
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        var paramsElement = (JsonElement)request.Params!;
        var toolParams = JsonSerializer.Deserialize<CallToolParams>(paramsElement.GetRawText());

        if (toolParams == null)
            throw new Exception("Invalid tool call parameters");

        var result = toolParams.Name switch
        {
            "calculate" => ExecuteCalculate(toolParams.Arguments),
            "echo" => ExecuteEcho(toolParams.Arguments),
            _ => throw new Exception($"Unknown tool: {toolParams.Name}")
        };

        return new CallToolResult
        {
            Content = new List<Content>
            {
                new Content { Type = "text", Text = result }
            }
        };
    }

    private string ExecuteCalculate(Dictionary<string, object>? arguments)
    {
        if (arguments == null)
            throw new Exception("Missing arguments");

        var operation = arguments["operation"].ToString();
        var a = Convert.ToDouble(((JsonElement)arguments["a"]).GetDouble());
        var b = Convert.ToDouble(((JsonElement)arguments["b"]).GetDouble());

        double result = operation switch
        {
            "add" => a + b,
            "subtract" => a - b,
            "multiply" => a * b,
            "divide" => b != 0 ? a / b : throw new Exception("Division by zero"),
            _ => throw new Exception($"Unknown operation: {operation}")
        };

        return $"Result: {result}";
    }

    private string ExecuteEcho(Dictionary<string, object>? arguments)
    {
        if (arguments == null)
            throw new Exception("Missing arguments");

        var message = arguments["message"].ToString();
        return $"Echo: {message}";
    }

    private ListResourcesResult HandleListResources()
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        return new ListResourcesResult { Resources = _resources };
    }

    private ReadResourceResult HandleReadResource(JsonRpcRequest request)
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        var paramsElement = (JsonElement)request.Params!;
        var resourceParams = JsonSerializer.Deserialize<ReadResourceParams>(paramsElement.GetRawText());

        if (resourceParams == null)
            throw new Exception("Invalid resource parameters");

        var content = resourceParams.Uri switch
        {
            "example://readme" => "This is a sample MCP server built with .NET Core.\n\nIt demonstrates:\n- Tool execution (calculate, echo)\n- Resource reading\n- Prompt retrieval",
            _ => throw new Exception($"Unknown resource: {resourceParams.Uri}")
        };

        return new ReadResourceResult
        {
            Contents = new List<ResourceContent>
            {
                new ResourceContent
                {
                    Uri = resourceParams.Uri,
                    MimeType = "text/plain",
                    Text = content
                }
            }
        };
    }

    private ListPromptsResult HandleListPrompts()
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        return new ListPromptsResult { Prompts = _prompts };
    }

    private GetPromptResult HandleGetPrompt(JsonRpcRequest request)
    {
        if (!_initialized)
            throw new Exception("Server not initialized");

        var paramsElement = (JsonElement)request.Params!;
        var promptParams = JsonSerializer.Deserialize<GetPromptParams>(paramsElement.GetRawText());

        if (promptParams == null)
            throw new Exception("Invalid prompt parameters");

        if (promptParams.Name == "greeting")
        {
            var name = promptParams.Arguments?.GetValueOrDefault("name", "World");
            return new GetPromptResult
            {
                Description = "A friendly greeting",
                Messages = new List<PromptMessage>
                {
                    new PromptMessage
                    {
                        Role = "user",
                        Content = new Content
                        {
                            Type = "text",
                            Text = $"Please provide a warm and friendly greeting for {name}."
                        }
                    }
                }
            };
        }

        throw new Exception($"Unknown prompt: {promptParams.Name}");
    }
}
