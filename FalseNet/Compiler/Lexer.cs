using FalseNet.Exceptions;
using FalseNet.Infrastructure;

namespace FalseNet.Compiler;

internal static class Lexer
{
    private static IEnumerable<Token> Lex(string code)
    {
        var buffer = new TextBuffer(code);
        var commentStack = new Stack<int>();

        while (buffer.MoveNext())
        {
            if (buffer.Peek() == '}')
            {
                if (!commentStack.TryPop(out _))
                {
                    throw new LexerException(buffer.Line, 
                        buffer.Column, 
                        "Unmatched comment close bracket '}'.");
                }
            }
            else if (commentStack.Count != 0)
            {
                continue;
            }
            
            switch (buffer.Peek())
            {
                case '{':
                    commentStack.Push(buffer.Position);
                    break;
                
                case var c when char.IsLetter(c):
                    yield return ScanIdentifier(buffer);
                    break;

                case var c when char.IsDigit(c):
                    yield return ScanNumber(buffer);
                    break;

                case '"':
                    yield return ScanLiteral(buffer);
                    break;

                case ';':
                    yield return new Token(TokenType.Semicolon, buffer.Position);
                    break;

                case '^':
                    yield return new Token(TokenType.Caret, buffer.Position);
                    break;

                case ':':
                    yield return new Token(TokenType.Colon, buffer.Position);
                    break;

                case '+':
                    yield return new Token(TokenType.Plus, buffer.Position);
                    break;

                case '-':
                    yield return new Token(TokenType.Minus, buffer.Position);
                    break;

                case '*':
                    yield return new Token(TokenType.Asterisk, buffer.Position);
                    break;

                case '/':
                    yield return new Token(TokenType.Slash, buffer.Position);
                    break;

                case '!':
                    yield return new Token(TokenType.Exclamation, buffer.Position);
                    break;

                case '?':
                    yield return new Token(TokenType.Question, buffer.Position);
                    break;

                case '=':
                    yield return new Token(TokenType.Equals, buffer.Position);
                    break;

                case '>':
                    yield return new Token(TokenType.GreaterThan, buffer.Position);
                    break;

                case '~':
                    yield return new Token(TokenType.Tilde, buffer.Position);
                    break;

                case '&':
                    yield return new Token(TokenType.Ampersand, buffer.Position);
                    break;

                case '|':
                    yield return new Token(TokenType.Bar, buffer.Position);
                    break;

                case '$':
                    yield return new Token(TokenType.Dollar, buffer.Position);
                    break;

                case '_':
                    yield return new Token(TokenType.Underscore, buffer.Position);
                    break;

                case '%':
                    yield return new Token(TokenType.Percent, buffer.Position);
                    break;

                case '\\':
                    yield return new Token(TokenType.Backslash, buffer.Position);
                    break;

                case '@':
                    yield return new Token(TokenType.At, buffer.Position);
                    break;

                case '.':
                    yield return new Token(TokenType.Dot, buffer.Position);
                    break;

                case ',':
                    yield return new Token(TokenType.Comma, buffer.Position);
                    break;

                case '[':
                    yield return new Token(TokenType.OpenSquareBracket, buffer.Position);
                    break;

                case ']':
                    yield return new Token(TokenType.CloseSquareBracket, buffer.Position);
                    break;

                case '#':
                    yield return new Token(TokenType.Hash, buffer.Position);
                    break;

                case 'ø':
                    yield return new Token(TokenType.Pick, buffer.Position);
                    break;

                case '§':
                    yield return new Token(TokenType.Section, buffer.Position);
                    break;
            }
        }
    }

    private static Token ScanLiteral(TextBuffer buffer)
    {
        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position + 1);

        while (buffer.PeekNext(out var nextChar) && nextChar is not '"')
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Literal, startPosition, endPosition - startPosition);
    }
    
    private static Token ScanNumber(TextBuffer buffer)
    {
        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position + 1);

        while (buffer.PeekNext(out var nextChar) && (char.IsDigit(nextChar) || nextChar is '.'))
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Number, startPosition, endPosition - startPosition);
    }

    private static Token ScanIdentifier(TextBuffer buffer)
    {
        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position + 1);

        while (buffer.PeekNext(out var nextChar) && (char.IsLetter(nextChar) || char.IsDigit(nextChar)))
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Identifier, startPosition, endPosition - startPosition);
    }
}