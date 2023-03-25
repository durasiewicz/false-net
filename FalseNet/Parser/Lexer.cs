using FalseNet.Exceptions;

namespace FalseNet.Parser;

public static class Lexer
{
    public static IEnumerable<Token> Lex(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentOutOfRangeException(nameof(input));
        }

        var commentStack = new Stack<Token>();

        var currentLine = 0;
        var currentPosition = 0;

        for (var index = 0; index < input.Length; index++)
        {
            switch (input[index])
            {
                case '{':
                {
                    var token = new Token(currentPosition, currentLine, TokenType.CommentBegin);
                    commentStack.Push(token);
                    break;
                }

                case '}':
                {
                    if (!commentStack.TryPop(out _))
                    {
                        throw new LexerException(currentPosition,
                            currentLine,
                            "Unexpected character '}'.");
                    }

                    break;
                }
                
                case '\n':
                    currentLine++;
                    currentPosition = 0;
                    break;
            }

            currentPosition++;
        }

        // Unclosed comment
        if (commentStack.Any())
        {
            var firstComment = commentStack.First();
            throw new LexerException(firstComment.posX,
                firstComment.posY,
                "Unclosed comment.");
        }

        return Enumerable.Empty<Token>();
    }
}