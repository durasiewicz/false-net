namespace FalseNet.Runtime;

public class ReferenceValue : StackValue
{
    public string Key { get; }

    public ReferenceValue(string key) : base(StackValueType.Reference)
    {
        Key = key;
    }
}