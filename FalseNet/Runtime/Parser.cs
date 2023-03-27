using FalseNet.Analyzers;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public static class Parser
{
    public static void Parse(IEnumerable<Token> tokens)
    {
        var evaluationStack = new EvaluationStack();
        var variables = new Dictionary<string, int>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    evaluationStack.PushNumber(int.Parse(token.Value));
                    break;

                case TokenType.PrintCharacter:
                {
                    var num = evaluationStack.PopNumber();
                    Console.Write((char)num.Value);
                    break;
                }
                
                case TokenType.PrintNumber:
                {
                    var num = evaluationStack.PopNumber();
                    Console.Write(num.Value);
                    break;
                }
                
                case TokenType.Duplicate:
                {
                    var num = evaluationStack.PopNumber();
                    evaluationStack.PushNumber(num.Value);
                    evaluationStack.PushNumber(num.Value);
                    break;
                }
                
                case TokenType.Swap:
                {
                    var num2 = evaluationStack.PopNumber();
                    var num1 = evaluationStack.PopNumber();
                    evaluationStack.PushNumber(num2.Value);
                    evaluationStack.PushNumber(num1.Value);
                    
                    break;
                }
                
                case TokenType.Addition:
                case TokenType.Substraction:
                case TokenType.Multiplication:
                case TokenType.Division:
                {
                    var num2 = evaluationStack.PopNumber();
                    var num1 = evaluationStack.PopNumber();

                    var result = token.Type switch
                    {
                        TokenType.Addition => num1.Value + num2.Value,
                        TokenType.Substraction => num1.Value - num2.Value,
                        TokenType.Multiplication => num1.Value * num2.Value,
                        TokenType.Division => num1.Value / num2.Value,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
                    evaluationStack.PushNumber(result);
                    
                    break;
                }

                case TokenType.ValueSet:
                {
                    var reference = evaluationStack.PopReference();
                    var value = evaluationStack.PopNumber();

                    if (!variables.ContainsKey(reference.Key))
                        variables.Add(reference.Key, 0);

                    variables[reference.Key] = value.Value;
                    break;
                }

                case TokenType.ValueFetch:
                {
                    var reference = evaluationStack.PopReference();

                    if (!variables.ContainsKey(reference.Key))
                        variables.Add(reference.Key, 0);
                    
                    evaluationStack.PushNumber(variables[reference.Key]);
                    
                    break;
                }

                case TokenType.Literal:
                    Console.Write(token.Value);
                    break;

                case TokenType.Variable:
                    evaluationStack.PushReference(token.Value);
                    break;
                
                default:
                    throw new RuntimeException($"Unsupported token type '{token.Type}'");
            }
        }
    }
}