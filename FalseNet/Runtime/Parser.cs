using FalseNet.Analyzers;
using FalseNet.Exceptions;

namespace FalseNet.Runtime;

public static class Parser
{
    public static void Parse(IEnumerable<Token> tokens)
    {
        var evaluationStack = new EvaluationStack();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Number:
                    evaluationStack.PushNumber(int.Parse(token.Value));
                    break;
                
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
                
                case TokenType.Literal:
                    Console.WriteLine(token.Value);
                    break;

                default:
                    throw new RuntimeException($"Unsupported token type '{token.Type}'");
            }
        }
    }
}