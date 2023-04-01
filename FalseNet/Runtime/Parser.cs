using FalseNet.Analyzers;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class Parser
{
    private readonly EvaluationStack _evaluationStack = new();
    private readonly Dictionary<string, Variable> _variables = new();
    private readonly Dictionary<int, List<Token>> _functions = new();

    private const string AnonymousFunctionPrefix = "<f>_";
    private const int TrueValue = -1;
    private const int FalseValue = 0;

    public void Parse(IEnumerable<Token> tokens)
    {
        tokens = CompileFunctions(tokens);

        foreach (var token in tokens)
        {
            try
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        _evaluationStack.PushNumber(int.Parse(token.Value));
                        break;

                    case TokenType.FunctionCall:
                    {
                        var functionHandle = _evaluationStack.PopNumber();
                        CallFunction(functionHandle);
                        break;
                    }

                    case TokenType.Loop:
                    {
                        var bodyFunctionHandle = _evaluationStack.PopNumber();
                        var conditionFunctionHandle = _evaluationStack.PopNumber();

                        while (true)
                        {
                            CallFunction(conditionFunctionHandle);

                            var conditionResult = _evaluationStack.PopNumber();

                            if (conditionResult.Value == TrueValue)
                            {
                                CallFunction(bodyFunctionHandle);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }

                    case TokenType.Equals:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num1.Value == num2.Value ? TrueValue : FalseValue);
                        break;
                    }

                    case TokenType.PrintCharacter:
                    {
                        var num = _evaluationStack.PopNumber();
                        Console.Write((char)num.Value);
                        break;
                    }

                    case TokenType.PrintNumber:
                    {
                        var num = _evaluationStack.PopNumber();
                        Console.Write(num.Value);
                        break;
                    }

                    case TokenType.Duplicate:
                    {
                        var num = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num.Value);
                        _evaluationStack.PushNumber(num.Value);
                        break;
                    }

                    case TokenType.GreaterThan:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num1.Value > num2.Value ? TrueValue : FalseValue);
                        break;
                    }

                    case TokenType.Negation:
                    {
                        var num = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num.Value == FalseValue ? TrueValue : FalseValue);

                        break;
                    }

                    case TokenType.Swap:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num2.Value);
                        _evaluationStack.PushNumber(num1.Value);

                        break;
                    }

                    case TokenType.MinusSign:
                    {
                        var num = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(-num.Value);
                        break;
                    }

                    case TokenType.Condition:
                    {
                        var functionId = _evaluationStack.PopNumber();
                        var condition = _evaluationStack.PopNumber();

                        if (condition.Value == TrueValue)
                        {
                            CallFunction(functionId);
                        }

                        break;
                    }

                    case TokenType.Delete:
                    {
                        _evaluationStack.PopAny();
                        break;
                    }

                    case TokenType.Addition:
                    case TokenType.Substraction:
                    case TokenType.Multiplication:
                    case TokenType.Division:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();

                        var result = token.Type switch
                        {
                            TokenType.Addition => num1.Value + num2.Value,
                            TokenType.Substraction => num1.Value - num2.Value,
                            TokenType.Multiplication => num1.Value * num2.Value,
                            TokenType.Division => num1.Value / num2.Value,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        _evaluationStack.PushNumber(result);

                        break;
                    }

                    case TokenType.ValueSet:
                    {
                        var reference = _evaluationStack.PopReference();
                        var value = _evaluationStack.PopNumber();
                        var variable = new Variable(value.Value, value.IsFunctionHandle);

                        _variables[reference.Key] = variable;

                        break;
                    }

                    case TokenType.ValueFetch:
                    {
                        var reference = _evaluationStack.PopReference();
                        _variables.TryAdd(reference.Key, new Variable(0, false));
                        _evaluationStack.PushNumber(_variables[reference.Key].Value,
                            _variables[reference.Key].IsFunctionHandle);

                        break;
                    }

                    case TokenType.FunctionBegin:
                    case TokenType.FunctionEnd:
                        throw new RuntimeException("Unexpected function begin or end. Functions compiler fails?");

                    case TokenType.Literal:
                        Console.Write(token.Value);
                        break;

                    case TokenType.Variable:
                        _evaluationStack.PushReference(token.Value);
                        break;

                    default:
                        throw new RuntimeException($"Unsupported token type '{token.Type}'");
                }
            }
            catch (RuntimeException e)
            {
                throw new RuntimeException($"{e.Message} @ line: {token.Line} position: {token.Position}");
            }
        }
    }

    private IEnumerable<Token> CompileFunctions(IEnumerable<Token> tokens)
    {
        var functionCounter = 0;
        var functionsHandleStack = new Stack<int>();
        var result = new List<Token>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.FunctionBegin:
                {
                    var functionHandle = functionCounter++;
                    functionsHandleStack.Push(functionHandle);
                    _functions.Add(functionHandle, new List<Token>());
                    break;
                }

                case TokenType.FunctionEnd:
                {
                    var functionHandle = functionsHandleStack.Pop();
                    var variableName = AnonymousFunctionPrefix + functionHandle;
                    var variableToken = token with { Type = TokenType.Variable, Value = variableName };
                    var fetchValueToken = token with { Type = TokenType.ValueFetch, Value = null };
                    
                    _variables.Add(variableName, new Variable(functionHandle, true));

                    if (functionsHandleStack.Any())
                    {
                        _functions[functionsHandleStack.Peek()].Add(variableToken);
                        _functions[functionsHandleStack.Peek()].Add(fetchValueToken);
                    }
                    else
                    {
                        result.Add(variableToken);
                        result.Add(fetchValueToken);
                    }
                    
                    break;
                }

                default:
                {
                    if (functionsHandleStack.Any())
                    {
                        _functions[functionsHandleStack.Peek()].Add(token);
                    }
                    else
                    {
                        result.Add(token);
                    }

                    break;
                }
            }
        }

        return result;
    }

    private void CallFunction(NumberValue functionId)
    {
        if (!functionId.IsFunctionHandle)
        {
            throw new RuntimeException("Value is not a function handle.");
        }

        if (!_functions.TryGetValue(functionId.Value, out var function))
        {
            throw new RuntimeException("Function is undefined.");
        }

        Parse(function);
    }
}