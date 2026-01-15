# mcplab

Model Context Protocol (MCP) Server Laboratory - A collection of MCP server implementations.

## Projects

### MCP Server (.NET Core)

A fully functional MCP server implementation using .NET Core that demonstrates:
- JSON-RPC 2.0 protocol handling
- Standard MCP capabilities (tools, resources, prompts)
- Stdio transport layer
- Example implementations of calculator and echo tools

See [src/McpServer/README.md](src/McpServer/README.md) for detailed documentation.

## Getting Started

### Prerequisites

- .NET SDK 10.0 or later

### Building

```bash
dotnet build
```

### Running

```bash
dotnet run --project src/McpServer
```

## Protocol Specification

This implementation follows the Model Context Protocol (MCP) specification, which enables communication between AI assistants and external tools/resources via a standardized JSON-RPC 2.0 interface.

## License

This is a sample implementation for educational purposes.