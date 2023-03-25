namespace FalseNet.Runtime;

public class StackValue
{
    public StackValueType Type { get; }

    protected StackValue(StackValueType type)
    {
        Type = type;
    }
}