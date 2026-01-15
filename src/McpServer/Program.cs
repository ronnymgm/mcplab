using System.Text;
using System.Text.Json;
using McpServer.Handlers;
using McpServer.Models;

var handler = new McpServerHandler();

// Log startup to stderr (for debugging)
Console.Error.WriteLine("MCP Server starting...");

try
{
    while (true)
    {
        var line = Console.ReadLine();
        if (line == null)
        {
            Console.Error.WriteLine("EOF reached, shutting down.");
            break;
        }

        if (string.IsNullOrWhiteSpace(line))
            continue;

        Console.Error.WriteLine($"Received: {line}");

        try
        {
            var request = JsonSerializer.Deserialize<JsonRpcRequest>(line);
            if (request == null)
            {
                Console.Error.WriteLine("Failed to deserialize request");
                continue;
            }

            JsonRpcResponse response;

            try
            {
                var result = handler.HandleRequest(request);
                response = new JsonRpcResponse
                {
                    Id = request.Id,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error handling request: {ex.Message}");
                response = new JsonRpcResponse
                {
                    Id = request.Id,
                    Error = new JsonRpcError
                    {
                        Code = -32603,
                        Message = ex.Message
                    }
                };
            }

            var responseJson = JsonSerializer.Serialize(response);
            Console.WriteLine(responseJson);
            Console.Error.WriteLine($"Sent: {responseJson}");
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"JSON parse error: {ex.Message}");
            var errorResponse = new JsonRpcResponse
            {
                Error = new JsonRpcError
                {
                    Code = -32700,
                    Message = "Parse error"
                }
            };
            Console.WriteLine(JsonSerializer.Serialize(errorResponse));
        }
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Fatal error: {ex}");
    return 1;
}

return 0;
