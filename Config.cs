using System.Globalization;
using System.Text;
using Fog.Core;

namespace Fog;

public class Config
{
    public List<FHeader> Headers { get; set; }
    private Parser _parser;
    
    public Config(string content)
    {
        _parser = new Parser();
        Headers = _parser.ParseFile(content);
    }

    public static Config FromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            string content = File.ReadAllText(fileName);
            return new Config(content);
        }
        else
            throw new Exception("File not found.");
    }

    public FHeader GetHeader(string header)
    {
        return Headers.Where(x => x.Name == header).FirstOrDefault();
    }

    public T GetKeyValue<T>(string header, string key) where T : struct
    {
        FHeader _header = GetHeader(header);
        foreach (var keyValue in _header.KeyValues)
        {
            if (keyValue.Name == key)
                return ConvertToGeneric<T>(keyValue.Value);
        }
        throw new Exception($"Key {key} not found in header {header}.");
    }

    public bool IsContainsKey(string header, string key)
    {
        FHeader _header = GetHeader(header);
        foreach (var keyValue in _header.KeyValues)
        {
            if (keyValue.Name == key)
                return true;
        }
        return false;
    }
    
    public bool IsContainsKey(string key)
    {
        foreach (var _header in Headers)
        {
            foreach (var keyValue in _header.KeyValues)
            {
                if (keyValue.Name == key)
                    return true;
            }
            return false;
        }

        return false;
    }
    
    public T GetKeyValue<T>(string key) where T : struct => GetKeyValue<T>("Global", key);
    public void SetKeyValue<T>(string key, T value) where T : struct => SetKeyValue("Global", key, value);

    public void Add(string header)
    {
        FHeader _header = GetHeader(header);
        if(_header == null)
            Headers.Add(new FHeader(header));
        else
            throw new Exception($"Header {header} already exists.");
    }

    public void Add<T>(string header, string key, T value) where T : struct
    {
        FHeader _header = GetHeader(header);
        if (_header.KeyValues.Where(x => x.Name == key).FirstOrDefault() == null)
        {
            _header.KeyValues.Add(new FKeyValue()
            {
                Name = key,
                Value = value.ToString()
            });
        }
    }
    
    public void Add<T>(string key, T value) where T : struct => Add("Global", key, value);
    
    public void SetKeyValue<T>(string header, string key, T value) where T : struct
    {
        FHeader _header = GetHeader(header);
        foreach (var keyValue in _header.KeyValues)
        {
            if (keyValue.Name == key)
            {
                keyValue.Value = value.ToString();
                return;
            }
        }
        throw new Exception($"Key {key} not found in header {header}.");
    }
    
    private T ConvertToGeneric<T>(string input) where T : struct
    {
        if (typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(decimal))
        {
            input = input.TrimEnd('f', 'F');
            return (T)Convert.ChangeType(input, typeof(T), CultureInfo.InvariantCulture);
        }
        if (typeof(T).IsEnum)
            return (T)Enum.Parse(typeof(T), input, ignoreCase: true);
        return (T)Convert.ChangeType(input, typeof(T), CultureInfo.InvariantCulture);
    }

    public String GenerateConfigAsString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var header in Headers)
        {
            sb.AppendLine($"[{header.Name}]");
            foreach (var keyValue in header.KeyValues)
            {
                sb.AppendLine($"{keyValue.Name}: {keyValue.Value}");
            }
            sb.AppendLine($"");
        }
        return sb.ToString();
    }
    
    public void Save(string fileName)
    {
        File.WriteAllText(fileName, GenerateConfigAsString());
    }
}