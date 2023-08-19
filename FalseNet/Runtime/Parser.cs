using System.Text;
using FalseNet.Exceptions;
using FalseNet.Lexing;

namespace FalseNet.Runtime;

internal class Parser
{
    private readonly EvaluationStack _evaluationStack = new();
    private readonly Dictionary<string, Variable> _variables = new();
    private readonly Dictionary<int, List<Token>> _functions = new();
    private readonly StringBuilder _inputBuffer = new();

    private const string AnonymousFunctionPrefix = "<f>_";
    private const int TrueValue = -1;
    private const int FalseValue = 0;

    public void Parse(string code, IEnumerable<Token> tokens)
    {
        var codeSpan = code.AsSpan();
        tokens = CompileFunctions(tokens);

        foreach (var token in tokens)
        {
            try
            {
                switch (token.Type)
                {
                    case TokenType.Section:
                        // no op - this is only for debugging purposes
                        var info = _evaluationStack.PrintDebugInfo();
                        break;
                    
                    case TokenType.Number:
                        _evaluationStack.PushNumber(int.Parse(codeSpan.Slice(token.Start, token.Lenght)));
                        break;

                    case TokenType.Exclamation:
                    {
                        var functionHandle = _evaluationStack.PopNumber();
                        CallFunction(code, functionHandle);
                        break;
                    }

                    case TokenType.Hash:
                    {
                        var bodyFunctionHandle = _evaluationStack.PopNumber();
                        var conditionFunctionHandle = _evaluationStack.PopNumber();

                        while (true)
                        {
                            CallFunction(code, conditionFunctionHandle);

                            var conditionResult = _evaluationStack.PopNumber();

                            if (conditionResult.Value == TrueValue)
                            {
                                CallFunction(code, bodyFunctionHandle);
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

                    case TokenType.Comma:
                    {
                        var num = _evaluationStack.PopNumber();
                        Console.Write((char)num.Value);
                        break;
                    }

                    case TokenType.Dot:
                    {
                        var num = _evaluationStack.PopNumber();
                        Console.Write(num.Value);
                        break;
                    }

                    case TokenType.Dollar:
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

                    case TokenType.Tilde:
                    {
                        var num = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num.Value == FalseValue ? TrueValue : FalseValue);

                        break;
                    }

                    case TokenType.Backslash:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(num2.Value);
                        _evaluationStack.PushNumber(num1.Value);

                        break;
                    }

                    case TokenType.Underscore:
                    {
                        var num = _evaluationStack.PopNumber();
                        _evaluationStack.PushNumber(-num.Value);
                        break;
                    }

                    case TokenType.Question:
                    {
                        var functionId = _evaluationStack.PopNumber();
                        var condition = _evaluationStack.PopNumber();

                        if (condition.Value == TrueValue)
                        {
                            CallFunction(code, functionId);
                        }

                        break;
                    }

                    case TokenType.Ampersand:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();

                        _evaluationStack.PushNumber(num1.Value == TrueValue && num2.Value == TrueValue
                            ? TrueValue
                            : FalseValue);

                        break;
                    }

                    case TokenType.Bar:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();

                        _evaluationStack.PushNumber(num1.Value == TrueValue || num2.Value == TrueValue
                            ? TrueValue
                            : FalseValue);

                        break;
                    }

                    case TokenType.Pick:
                    {
                        var number = _evaluationStack.PopNumber();
                        var item = _evaluationStack.PeekAny(number.Value);
                        _evaluationStack.PushAny(item);

                        break;
                    }

                    case TokenType.Percent:
                    {
                        _evaluationStack.PopAny();
                        break;
                    }

                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Asterisk:
                    case TokenType.Slash:
                    {
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();

                        var result = token.Type switch
                        {
                            TokenType.Plus => num1.Value + num2.Value,
                            TokenType.Minus => num1.Value - num2.Value,
                            TokenType.Asterisk => num1.Value * num2.Value,
                            TokenType.Slash => num1.Value / num2.Value,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        _evaluationStack.PushNumber(result);

                        break;
                    }

                    case TokenType.Colon:
                    {
                        var reference = _evaluationStack.PopReference();
                        var value = _evaluationStack.PopNumber();
                        var variable = new Variable(value.Value, value.IsFunctionHandle);

                        _variables[reference.Key] = variable;

                        break;
                    }

                    case TokenType.Semicolon:
                    {
                        var reference = _evaluationStack.PopReference();
                        _variables.TryAdd(reference.Key, new Variable(0, false));
                        _evaluationStack.PushNumber(_variables[reference.Key].Value,
                            _variables[reference.Key].IsFunctionHandle);

                        break;
                    }

                    case TokenType.At:
                    {
                        var num3 = _evaluationStack.PopNumber();
                        var num2 = _evaluationStack.PopNumber();
                        var num1 = _evaluationStack.PopNumber();

                        _evaluationStack.PushNumber(num2.Value);
                        _evaluationStack.PushNumber(num3.Value);
                        _evaluationStack.PushNumber(num1.Value);

                        break;
                    }

                    case TokenType.Caret:
                    {
                        // if buffer is empty take some input from user
                        while (_inputBuffer.Length == 0)
                        {
                            _inputBuffer.Append(Console.ReadLine());
                            _inputBuffer.Append('\n');
                        }

                        var character = _inputBuffer[0];
                        _inputBuffer.Remove(0, 1);

                        _evaluationStack.PushNumber(character);
                        break;
                    }

                    case TokenType.Literal:
                        Console.Out.Write(codeSpan.Slice(token.Start, token.Lenght));
                        break;

                    case TokenType.Identifier:
                        _evaluationStack.PushReference(token.Value ?? codeSpan.Slice(token.Start, token.Lenght).ToString());
                        break;

                    default:
                        throw new RuntimeException($"Unsupported token type '{token.Type}'");
                }
            }
            catch (RuntimeException e)
            {
                throw new RuntimeException($"{e.Message} @ line: {token.Line} column: {token.Column}");
            }
        }
    }

    private IEnumerable<Token> CompileFunctions(IEnumerable<Token> tokens)
    {
        var functionCounter = 0;
        var functionsHandleStack = new Stack<int>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.OpenSquareBracket:
                {
                    var functionHandle = functionCounter++;
                    functionsHandleStack.Push(functionHandle);
                    _functions.Add(functionHandle, new List<Token>());
                    break;
                }

                case TokenType.CloseSquareBracket:
                {
                    var functionHandle = functionsHandleStack.Pop();
                    var variableName = AnonymousFunctionPrefix + functionHandle;
                    var variableToken = token with { Type = TokenType.Identifier, Value = variableName };
                    var fetchValueToken = token with { Type = TokenType.Semicolon };

                    _variables.Add(variableName, new Variable(functionHandle, true));

                    if (functionsHandleStack.Count != 0)
                    {
                        _functions[functionsHandleStack.Peek()].Add(variableToken);
                        _functions[functionsHandleStack.Peek()].Add(fetchValueToken);
                    }
                    else
                    {
                        yield return variableToken;
                        yield return fetchValueToken;
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
                        yield return token;
                    }

                    break;
                }
            }
        }
    }

    private void CallFunction(string code, NumberValue functionId)
    {
        if (!functionId.IsFunctionHandle)
        {
            throw new RuntimeException("Value is not a function handle.");
        }

        if (!_functions.TryGetValue(functionId.Value, out var function))
        {
            throw new RuntimeException("Function is undefined.");
        }

        Parse(code, function);
    }
}