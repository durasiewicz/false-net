using System.Text;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class EvaluationStack
{
    private readonly Stack<StackValue> _stack = new();

    public void PushNumber(int value, bool isFunctionHandle = false)
    {
        _stack.Push(new NumberValue(value, isFunctionHandle));
    }

    public void PushAny(StackValue value) => _stack.Push(value);

    public StackValue PeekAny(int index)
    {
        if (index >= _stack.Count)
        {
            throw new RuntimeException($"Index '{index}' is out of evaluation stack range.");
        }

        return _stack.ElementAt(index);
    }

    public StackValue PopAny()
    {
        if (!_stack.TryPop(out var value))
        {
            throw new RuntimeException("Evaluation stack is empty.");
        }

        return value;
    }

    public NumberValue PopNumber()
    {
        var value = PopAny();

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
        var value = PopAny();

        if (value.Type is not StackValueType.Reference)
        {
            throw new RuntimeException(
                $"Expected '{nameof(StackValueType.Reference)}' on stack. Got '{value.Type.ToString()}'.");
        }

        return (ReferenceValue)value;
    }

    public string PrintDebugInfo()
    {
        var sb = new StringBuilder();

        foreach (var value in _stack)
        {
            var msg = value switch
            {
                NumberValue numberValue => $"{numberValue.Type} {numberValue.Value} {(char)numberValue.Value}",
                ReferenceValue refValue => $"{refValue.Type} {refValue.Key}"
            };

            sb.AppendLine(msg);
        }

        return sb.ToString();
    }
}