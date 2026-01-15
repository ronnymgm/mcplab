#!/bin/bash
# Example test script for the MCP server

echo "Starting MCP Server test..."
echo ""

# Start the server and send test commands
(
cat << 'EOF'
{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"example-client","version":"1.0.0"}}}
{"jsonrpc":"2.0","method":"initialized"}
{"jsonrpc":"2.0","id":2,"method":"tools/list"}
{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"calculate","arguments":{"operation":"add","a":10,"b":5}}}
{"jsonrpc":"2.0","id":4,"method":"tools/call","params":{"name":"echo","arguments":{"message":"Hello from MCP!"}}}
{"jsonrpc":"2.0","id":5,"method":"resources/list"}
{"jsonrpc":"2.0","id":6,"method":"resources/read","params":{"uri":"example://readme"}}
{"jsonrpc":"2.0","id":7,"method":"prompts/list"}
{"jsonrpc":"2.0","id":8,"method":"prompts/get","params":{"name":"greeting","arguments":{"name":"World"}}}
EOF
) | dotnet run --project src/McpServer 2>&1

echo ""
echo "Test completed!"
