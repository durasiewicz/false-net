namespace FalseNet.Runtime;

public class ReferenceStackValue : StackValue
{
    public string Key { get; }

    public ReferenceStackValue(string key)
    {
        Key = key;
    }
}