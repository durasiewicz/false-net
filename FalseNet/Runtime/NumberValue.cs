namespace FalseNet.Runtime;

public class NumberValue : StackValue
{
    public int Value { get; }

    public NumberValue(int value) : base(StackValueType.Number)
    {
        Value = value;
    }
}