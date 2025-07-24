using System.Collections;

namespace Fog.Core;

public class FHeader : FObject
{
    public List<FKeyValue> KeyValues { get; set; }
    
    public FHeader(string name)
    {
        Name = name;
        KeyValues = new List<FKeyValue>();
    }
}