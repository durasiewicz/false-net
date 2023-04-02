using System.Text;
using FalseNet.Exceptions;

namespace FalseNet.Analyzers;

public static class Lexer
{
    private enum Mode
    {
        Default,
        DoubleQuotedStringLiteral,
        NumericLiteral,
        StringLiteral
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

            if (mode == Mode.DoubleQuotedStringLiteral && input[index] is not '"')
            {
                charBuffer.Append(input[index]);
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

                case ';':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Semicolon);
                    break;
                
                case '^':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Caret);
                    break;
                
                case ':':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Colon);
                    break;
                
                case '+':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Plus);
                    break;
                
                case '-':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Minus);
                    break;
                
                case '*':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Asterisk);
                    break;
                
                case '/':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Slash);
                    break;
                
                case '!':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Exclamation);
                    break;
                
                case '?':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Question);
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
                        TokenType.Tilde);
                    break;
                
                case '&':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Ampersand);
                    break;
                
                case '|':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Bar);
                    break;
                
                case '$':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Dollar);
                    break;
                
                case '_':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Underscore);
                    break;
                
                case '%':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Percent);
                    break;
                
                case '\\':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Backslash);
                    break;
                
                case '@':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.At);
                    break;
                
                case '.':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Dot);
                    break;
                
                case ',':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Comma);
                    break;
                
                case '[':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.OpenBracket);
                    break;
                
                case ']':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.CloseBracket);
                    break;
                
                case '#':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Hash);
                    break;
                
                case '"':
                    switch (mode)
                    {
                        case Mode.Default:
                            mode = Mode.DoubleQuotedStringLiteral;
                            break;
                        case Mode.DoubleQuotedStringLiteral:
                            yield return new Token(currentPosition,
                                currentLine,
                                TokenType.DoubleQuotedStringLiteral,
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
                        case Mode.Default:
                            if (char.IsDigit(input[index]))
                                mode = Mode.NumericLiteral;

                            if (char.IsLetter(input[index]))
                                mode = Mode.StringLiteral;
                            break;
                    }

                    if (mode == Mode.NumericLiteral)
                    {
                        charBuffer.Append(input[index]);
                        
                        if (index + 1 >= input.Length || !char.IsNumber(input[index + 1]))
                        {
                            yield return new Token(currentPosition,
                                currentLine,
                                TokenType.NumericLiteral,
                                charBuffer.ToString());
                            
                            charBuffer.Clear();
                            mode = Mode.Default;
                        }
                    }
                    
                    if (mode == Mode.StringLiteral)
                    {
                        charBuffer.Append(input[index]);
                        
                        if (index + 1 < input.Length || !char.IsLetter(input[index + 1]))
                        {
                            yield return new Token(currentPosition,
                                currentLine,
                                TokenType.StringLiteral,
                                charBuffer.ToString());
                            
                            charBuffer.Clear();
                            mode = Mode.Default;
                        }
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