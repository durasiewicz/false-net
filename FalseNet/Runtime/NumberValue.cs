namespace FalseNet.Runtime;

public class NumberValue : StackValue
{
    public int Value { get; }
    public bool IsFunctionHandle { get; }

    public NumberValue(int value, bool isFunctionHandle) : base(StackValueType.Number)
    {
        Value = value;
        IsFunctionHandle = isFunctionHandle;
    }
}