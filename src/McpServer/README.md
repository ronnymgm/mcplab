# MCP Server (.NET Core)

A Model Context Protocol (MCP) server implementation using .NET Core.

## Features

This MCP server implements the core MCP protocol capabilities:

### Tools
- **calculate**: Performs basic arithmetic operations (add, subtract, multiply, divide)
- **echo**: Echoes back the provided message

### Resources
- **example://readme**: Server information and documentation

### Prompts
- **greeting**: Generates a friendly greeting with customizable name

## Building the Server

```bash
dotnet build
```

## Running the Server

The server communicates over stdio using JSON-RPC 2.0 protocol:

```bash
dotnet run --project src/McpServer
```

## Testing the Server

You can test the server by sending JSON-RPC messages via stdin. Here are some examples:

### Initialize the Server

```json
{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test-client","version":"1.0.0"}}}
```

### Send Initialized Notification

```json
{"jsonrpc":"2.0","method":"initialized"}
```

### List Available Tools

```json
{"jsonrpc":"2.0","id":2,"method":"tools/list"}
```

### Call the Calculator Tool

```json
{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"calculate","arguments":{"operation":"add","a":5,"b":3}}}
```

### Call the Echo Tool

```json
{"jsonrpc":"2.0","id":4,"method":"tools/call","params":{"name":"echo","arguments":{"message":"Hello, MCP!"}}}
```

### List Resources

```json
{"jsonrpc":"2.0","id":5,"method":"resources/list"}
```

### Read a Resource

```json
{"jsonrpc":"2.0","id":6,"method":"resources/read","params":{"uri":"example://readme"}}
```

### List Prompts

```json
{"jsonrpc":"2.0","id":7,"method":"prompts/list"}
```

### Get a Prompt

```json
{"jsonrpc":"2.0","id":8,"method":"prompts/get","params":{"name":"greeting","arguments":{"name":"Alice"}}}
```

## Using with MCP Clients

This server can be used with any MCP client by configuring it to run the server executable and communicate via stdio.

### Example Configuration

```json
{
  "mcpServers": {
    "dotnet-server": {
      "command": "dotnet",
      "args": ["run", "--project", "path/to/src/McpServer"]
    }
  }
}
```

## Protocol Compliance

This implementation follows the MCP protocol specification version 2024-11-05 and supports:

- JSON-RPC 2.0 message format
- Standard MCP methods (initialize, tools/*, resources/*, prompts/*)
- Error handling with proper error codes
- Stdio transport layer

## Architecture

```
src/McpServer/
├── Models/
│   ├── JsonRpcMessage.cs    # JSON-RPC 2.0 message types
│   └── McpModels.cs          # MCP protocol models
├── Handlers/
│   └── McpServerHandler.cs   # Request handling logic
└── Program.cs                # Stdio transport and main loop
```

## Development

To add new tools, resources, or prompts:

1. Add the definition to the appropriate `RegisterDefault*` method in `McpServerHandler.cs`
2. Implement the handler logic in the corresponding `Handle*` method
3. Add the case to the method switch statement in `HandleRequest`

## License

This is a sample implementation for educational purposes.
