using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class EvaluationStack
{
    private readonly Stack<StackValue> _stack = new Stack<StackValue>();
    
    public void PushNumber(int value, bool isFunctionHandle = false)
    {
        _stack.Push(new NumberValue(value, isFunctionHandle));
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

    public void PushReference(string key)
    {
        _stack.Push(new ReferenceValue(key));
    }
    
    public ReferenceValue PopReference()
    {
        if (!_stack.TryPop(out var value))
        {
            throw new RuntimeException("Runtime stack is empty.");
        }

        if (value.Type is not StackValueType.Reference)
        {
            throw new RuntimeException(
                $"Expected '{nameof(StackValueType.Reference)}' on stack. Got '{value.Type.ToString()}'.");
        }

        return (ReferenceValue)value;
    }
}