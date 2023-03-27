using System.Text;
using FalseNet.Exceptions;

namespace FalseNet.Analyzers;

public static class Lexer
{
    private enum Mode
    {
        Default,
        Literal,
        Number,
        Variable
    }

    public static IEnumerable<Token> Lex(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentOutOfRangeException(nameof(input));
        }

        var commentStack = new Stack<Token>();
        var currentLine = 0;
        var currentPosition = 0;
        var charBuffer = new StringBuilder();
        var mode = Mode.Default;

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
                    switch (mode)
                    {
                        case Mode.Default:
                            mode = Mode.Literal;
                            break;
                        case Mode.Literal:
                            yield return new Token(currentPosition,
                                currentLine,
                                TokenType.Literal,
                                charBuffer.ToString());

                            charBuffer.Clear();
                            mode = Mode.Default;
                            break;
                    }
                    break;
                
                case '\n':
                    currentLine++;
                    currentPosition = 0;
                    break;

                default:
                {
                    switch (mode)
                    {
                        case Mode.Literal:
                            charBuffer.Append(input[index]);
                            continue;
                        
                        case Mode.Default:
                            if (char.IsDigit(input[index]))
                                mode = Mode.Number;

                            if (char.IsLetter(input[index]))
                                mode = Mode.Variable;
                            break;
                    }

                    if (char.IsDigit(input[index]) || char.IsLetter(input[index]))
                    {
                        charBuffer.Append(input[index]);
                        
                        // Continue to collect entire number to buffer, then flush buffer to token
                        if (index + 1 < input.Length && 
                            (char.IsDigit(input[index + 1]) ||
                            char.IsLetter(input[index + 1])))
                        {
                            continue;
                        }
                        
                        yield return new Token(currentPosition,
                            currentLine,
                            mode switch
                            {
                                Mode.Number => TokenType.Number,
                                Mode.Variable => TokenType.Variable,
                                _ => throw new InvalidOperationException()
                            },
                            charBuffer.ToString());

                        charBuffer.Clear();
                        mode = Mode.Default;
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