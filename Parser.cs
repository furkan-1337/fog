using Fog.Core;

namespace Fog;

public class Parser
{
    public List<FHeader> ParseFile(string input)
    {
        var list = new List<FHeader>();
        list.Add(new FHeader("Global"));
        string[] lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            FObject obj = Parse(lines[i]);
            if (obj != null)
            {
                if (obj.GetType() == typeof(FHeader))
                    list.Add(obj as FHeader);
                else if (obj.GetType() == typeof(FKeyValue))
                    list.Where(x => x.Name == _lastHeader).FirstOrDefault().KeyValues.Add(obj as FKeyValue);
            }
        }
        return list;
    }
    
    private string _lastHeader = "Global";
    public FObject Parse(string line)
    {
        bool isHeader = line.StartsWith('[') && line.EndsWith(']');
        bool isKeyValue = line.Contains(':');
        if (isKeyValue)
        {
            string key = line.Split(':')[0].TrimStart();
            string value = line.Split(':')[1].TrimStart();
            return new FKeyValue()
            {
                Name = key,
                Value = value
            };
        }
        else if (isHeader)
        {
            string headerContent = line.Trim('[', ']');
            _lastHeader = headerContent;
            return new FHeader(headerContent);
        }
        else { return null; }
    }
}