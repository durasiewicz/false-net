namespace FalseNet.Runtime;

public class NumberStackValue : StackValue
{
    public int Value { get; }
    public bool IsFunctionHandle { get; }

    public NumberStackValue(int value, bool isFunctionHandle)
    {
        Value = value;
        IsFunctionHandle = isFunctionHandle;
    }
}