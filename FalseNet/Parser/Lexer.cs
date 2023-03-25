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
            if (commentStack.Any() && input[index] is not '{' and not '}')
            {
                continue;
            }
            
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

                case ':':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.ValueFetch);
                    break;
                
                case ';':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.ValueSet);
                    break;
                
                case '+':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Addition);
                    break;
                
                case '-':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Substraction);
                    break;
                
                case '*':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Multiplication);
                    break;
                
                case '/':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Division);
                    break;
                
                case '!':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.FunctionCall);
                    break;
                
                case '=':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Equals);
                    break;
                
                case '>':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.GreaterThan);
                    break;
                
                case '~':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Negation);
                    break;
                
                case '&':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.And);
                    break;
                
                case '|':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Or);
                    break;
                
                case '$':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Duplicate);
                    break;
                
                case '%':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Delete);
                    break;
                
                case '\\':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Swap);
                    break;
                
                case '@':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Rotate);
                    break;
                
                case '"':
                {
                    break;
                }
                
                case '\n':
                    currentLine++;
                    currentPosition = 0;
                    break;

                default:
                {
                    if (char.IsDigit(input[index]))
                    {
                        yield return new Token(currentPosition,
                            currentLine,
                            TokenType.Number,
                            input[index]);
                    }
                    
                    break;
                }
            }

            currentPosition++;
        }

        // Unclosed comment
        if (commentStack.Any())
        {
            var firstComment = commentStack.First();
            throw new LexerException(firstComment.Position,
                firstComment.Line,
                "Unclosed comment.");
        }
    }
}