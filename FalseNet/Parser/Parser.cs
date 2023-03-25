using FalseNet.Exceptions;
using FalseNet.Runtime;

namespace FalseNet.Parser;

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
                    evaluationStack.PushNumber(int.Parse(((char)token.Value).ToString()));
                    break;
                
                case TokenType.Addition:
                    var num1 = evaluationStack.PopNumber();
                    var num2 = evaluationStack.PopNumber();
                    evaluationStack.PushNumber(num1.Value + num2.Value);
                    break;
                
                default:
                    throw new RuntimeException($"Unsupported token type '{token.Type}'");
            }
        }
    }
}