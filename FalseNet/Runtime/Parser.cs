using FalseNet.Analyzers;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public class Parser
{
    private static int _functionCounter;
    private static EvaluationStack _evaluationStack = new EvaluationStack();
    private static Dictionary<string, int> _variables = new Dictionary<string, int>();
    private static Dictionary<int, List<Token>> _functions = new Dictionary<int, List<Token>>();
    private static Stack<int> _functionStack = new Stack<int>();

    private const int TrueValue = -1;
    private const int FalseValue = 0;
    
    public void Parse(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            if (_functionStack.Count > 0 &&
                token.Type is not TokenType.FunctionBegin
                    and not TokenType.FunctionEnd)
            {
                _functions[_functionStack.Peek()].Add(token);
                continue;
            }

            switch (token.Type)
            {
                case TokenType.Number:
                    _evaluationStack.PushNumber(int.Parse(token.Value));
                    break;

                case TokenType.FunctionCall:
                {
                    var functionId = _evaluationStack.PopNumber();
                    CallFunction(functionId);
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

                    if (!_variables.ContainsKey(reference.Key))
                        _variables.Add(reference.Key, 0);

                    _variables[reference.Key] = value.Value;
                    break;
                }

                case TokenType.ValueFetch:
                {
                    var reference = _evaluationStack.PopReference();

                    if (!_variables.ContainsKey(reference.Key))
                        _variables.Add(reference.Key, 0);

                    _evaluationStack.PushNumber(_variables[reference.Key]);

                    break;
                }

                case TokenType.FunctionBegin:
                {
                    var functionId = _functionCounter++;
                    _functionStack.Push(functionId);
                    _functions.Add(functionId, new List<Token>());
                    break;
                }

                case TokenType.FunctionEnd:
                {
                    var functionId = _functionStack.Pop();
                    _evaluationStack.PushNumber(functionId);
                    break;
                }

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
    }

    private void CallFunction(NumberValue functionId)
    {
        if (!_functions.TryGetValue(functionId.Value, out var function))
        {
            throw new RuntimeException("Function is undefined.");
        }

        Parse(function);
    }
}