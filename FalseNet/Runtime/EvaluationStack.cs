using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class EvaluationStack
{
    private readonly Stack<StackValue> _stack = new Stack<StackValue>();
    
    public void PushNumber(int value)
    {
        _stack.Push(new NumberValue(value));
    }

    public NumberValue PopNumber()
    {
        if (!_stack.TryPop(out var value))
        {
            throw new RuntimeException("Runtime stack is empty.");
        }

        if (value.Type is not StackValueType.Number)
        {
            throw new RuntimeException(
                $"Expected '{nameof(StackValueType.Number)}' on stack. Got '{value.Type.ToString()}'.");
        }

        return (NumberValue)value;
    }
}