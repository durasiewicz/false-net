using System.Text;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class EvaluationStack
{
    private readonly Stack<StackValue> _stack = new();

    public void PushNumber(int value, bool isFunctionHandle = false)
    {
        _stack.Push(new NumberStackValue(value, isFunctionHandle));
    }

    public void PushAny(StackValue value) => _stack.Push(value);

    public StackValue PeekAny(int index)
    {
        if (index >= _stack.Count)
        {
            throw new RuntimeException($"Index '{index}' is out of evaluation stack range");
        }

        return _stack.ElementAt(index);
    }

    public StackValue PopAny()
    {
        if (!_stack.TryPop(out var value))
        {
            throw new RuntimeException("Evaluation stack is empty");
        }

        return value;
    }

    public NumberStackValue PopNumber()
    {
        var value = PopAny();

        if (value is NumberStackValue numberStackValue)
        {
            return numberStackValue;
        }

        throw new RuntimeException(
            $"Expected '{nameof(NumberStackValue)}' on stack, got '{value.GetType()}'");
    }

    public void PushReference(string key)
    {
        _stack.Push(new ReferenceStackValue(key));
    }

    public ReferenceStackValue PopReference()
    {
        var value = PopAny();

        if (value is ReferenceStackValue referenceStackValue)
        {
            return referenceStackValue;
        }

        throw new RuntimeException(
            $"Expected '{nameof(ReferenceStackValue)}' on stack, got '{value.GetType()}'");
    }

    public string PrintDebugInfo()
    {
        var sb = new StringBuilder();

        foreach (var value in _stack)
        {
            var msg = value switch
            {
                NumberStackValue numberValue => $"{numberValue.GetType()} {numberValue.Value} {(char)numberValue.Value}",
                ReferenceStackValue refValue => $"{refValue.GetType()} {refValue.Key}",
                _ => throw new ArgumentOutOfRangeException()
            };

            sb.AppendLine(msg);
        }

        return sb.ToString();
    }
}