# fog - Furkan's Object Grammar

A simple and lightweight configuration file parser and manager for .NET applications.

## Features

- Parse configuration files with header-based structure
- Get and set key-value pairs with generic type support
- Add new headers and key-value pairs dynamically
- Check if keys exist in specific headers or globally
- Save configuration back to file
- Support for various data types including enums and numeric types

## Usage

### Basic Example

```csharp
// Load configuration from file content
Config config = Config.FromFile("config.fog");

// Get values with type conversion
int port = config.GetKeyValue<int>("Server", "Port");
string host = config.GetKeyValue<string>("Server", "Host");

// Get values from Global header (shorthand)
bool debug = config.GetKeyValue<bool>("Debug");

// Check if key exists
if (config.IsContainsKey("Server", "Port"))
{
    // Key exists in Server header
}

// Set existing values
config.SetKeyValue("Server", "Port", 8080);
config.SetKeyValue("Debug", false); // Sets in Global header

// Save configuration
config.Save("config.fog");
```

### Configuration File Format

```ini
[Global]
Debug: true
Timeout: 30

[Server]
Host: localhost
Port: 8080
```

## API Reference

### Constructor
- `Config(string content)` - Initialize with configuration file content

### Getting Values
- `GetKeyValue<T>(string header, string key)` - Get value from specific header
- `GetKeyValue<T>(string key)` - Get value from Global header

### Setting Values
- `SetKeyValue<T>(string header, string key, T value)` - Set value in specific header
- `SetKeyValue<T>(string key, T value)` - Set value in Global header

### Adding Elements
- `Add(string header)` - Add new header
- `Add<T>(string header, string key, T value)` - Add key-value to specific header
- `Add<T>(string key, T value)` - Add key-value to Global header

### Checking Existence
- `IsContainsKey(string header, string key)` - Check if key exists in header
- `IsContainsKey(string key)` - Check if key exists globally

### File Operations
- `Save(string fileName)` - Save configuration to file
- `GenerateConfigAsString()` - Generate configuration as string

## Requirements

- .NET Framework or .NET Core

## License

This project is open source. Please check the license file for more details.
