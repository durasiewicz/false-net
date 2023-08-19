using System.Text;
using FalseNet.Compiler;
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

        var pairedTokensStack = new Stack<Token>();
        var currentLine = 1;
        var currentPosition = 1;
        var charBuffer = new StringBuilder();
        var mode = Mode.Default;

        for (var index = 0; index < input.Length; index++)
        {
            if (pairedTokensStack.Any() && 
                pairedTokensStack.Peek().Type is TokenType.CommentBegin &&
                input[index] is not '{' and not '}')
            {
                if (input[index] == '\n')
                {
                    currentPosition = 1;
                    currentLine++;
                }
                
                currentPosition++;
                
                continue;
            }

            if (mode == Mode.DoubleQuotedStringLiteral && input[index] is not '"')
            {
                charBuffer.Append(input[index]);
                
                if (input[index] == '\n')
                {
                    currentPosition = 1;
                    currentLine++;
                }

                currentPosition++;
                
                continue;
            }

            switch (input[index])
            {
                case '{':
                {
                    var token = new Token(currentPosition, currentLine, TokenType.CommentBegin);
                    pairedTokensStack.Push(token);
                    break;
                }

                case '}':
                {
                    if (!pairedTokensStack.TryPop(out var token) || token.Type is not TokenType.CommentBegin)
                    {
                        throw new LexerException(currentLine,
                            currentPosition, "Unexpected character '}'");
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
                        TokenType.OpenSquareBracket);
                    break;
                
                case ']':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.CloseSquareBracket);
                    break;
                
                case '#':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Hash);
                    break;
                
                case 'ø':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Pick);
                    break;
                
                case '§':
                    yield return new Token(currentPosition,
                        currentLine,
                        TokenType.Section);
                    break;
                
                case '"':
                    switch (mode)
                    {
                        case Mode.Default:
                        {
                            mode = Mode.DoubleQuotedStringLiteral;
                            pairedTokensStack.Push(new Token(currentPosition,
                                currentLine,
                                TokenType.DoubleQuotedStringLiteral));
                            break;
                        }
                        case Mode.DoubleQuotedStringLiteral:
                        {
                            if (!pairedTokensStack.TryPop(out var token) ||
                                token.Type is not TokenType.DoubleQuotedStringLiteral)
                            {
                                throw new LexerException(currentLine,
                                    currentPosition, "Unexpected character '\"'");
                            }

                            yield return token with { Value = charBuffer.ToString() };
                            charBuffer.Clear();
                            mode = Mode.Default;
                            break;
                        }
                    }
                    break;
                
                case '\n':
                {
                    currentLine++;
                    currentPosition = 1;
                    break;
                }

                default:
                {
                    if (mode is Mode.Default)
                    {
                        if (char.IsDigit(input[index]))
                        {
                            mode = Mode.NumericLiteral;
                        } 
                        else if (char.IsLetter(input[index]))
                        {
                            mode = Mode.StringLiteral;
                        }
                    }

                    switch (mode)
                    {
                        case Mode.NumericLiteral:
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

                            break;
                        }
                        
                        case Mode.StringLiteral:
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

                            break;
                        }
                    }

                    break;
                }
            }

            currentPosition++;
        }

        // Unclosed tokens
        if (pairedTokensStack.Any())
        {
            var unclosedToken = pairedTokensStack.Pop();

            throw new LexerException(unclosedToken.Line, unclosedToken.Position, unclosedToken.Type switch
            {
                TokenType.CommentBegin => "Unclosed comment",
                TokenType.DoubleQuotedStringLiteral => "Unclosed double-quoted string literal",
                var type => $"Unclosed token of type '{type}'"
            });
        }
    }
}