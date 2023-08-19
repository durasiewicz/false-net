using FalseNet.Exceptions;
using FalseNet.Infrastructure;

namespace FalseNet.Lexing;

internal static class Lexer
{
    private const char Pick = 'ø';
    
    public static IEnumerable<Token> Lex(string code)
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

                case var c when char.IsLetter(c) && c is not Pick:
                    yield return ScanIdentifier(buffer);
                    break;

                case var c when char.IsDigit(c):
                    yield return ScanNumber(buffer);
                    break;

                case '"':
                    yield return ScanLiteral(buffer);
                    break;

                case ';':
                    yield return new Token(TokenType.Semicolon, buffer);
                    break;

                case '^':
                    yield return new Token(TokenType.Caret, buffer);
                    break;

                case ':':
                    yield return new Token(TokenType.Colon, buffer);
                    break;

                case '+':
                    yield return new Token(TokenType.Plus, buffer);
                    break;

                case '-':
                    yield return new Token(TokenType.Minus, buffer);
                    break;

                case '*':
                    yield return new Token(TokenType.Asterisk, buffer);
                    break;

                case '/':
                    yield return new Token(TokenType.Slash, buffer);
                    break;

                case '!':
                    yield return new Token(TokenType.Exclamation, buffer);
                    break;

                case '?':
                    yield return new Token(TokenType.Question, buffer);
                    break;

                case '=':
                    yield return new Token(TokenType.Equals, buffer);
                    break;

                case '>':
                    yield return new Token(TokenType.GreaterThan, buffer);
                    break;

                case '~':
                    yield return new Token(TokenType.Tilde, buffer);
                    break;

                case '&':
                    yield return new Token(TokenType.Ampersand, buffer);
                    break;

                case '|':
                    yield return new Token(TokenType.Bar, buffer);
                    break;

                case '$':
                    yield return new Token(TokenType.Dollar, buffer);
                    break;

                case '_':
                    yield return new Token(TokenType.Underscore, buffer);
                    break;

                case '%':
                    yield return new Token(TokenType.Percent, buffer);
                    break;

                case '\\':
                    yield return new Token(TokenType.Backslash, buffer);
                    break;

                case '@':
                    yield return new Token(TokenType.At, buffer);
                    break;

                case '.':
                    yield return new Token(TokenType.Dot, buffer);
                    break;

                case ',':
                    yield return new Token(TokenType.Comma, buffer);
                    break;

                case '[':
                    yield return new Token(TokenType.OpenSquareBracket, buffer);
                    break;

                case ']':
                    yield return new Token(TokenType.CloseSquareBracket, buffer);
                    break;

                case '#':
                    yield return new Token(TokenType.Hash, buffer);
                    break;

                case Pick:
                    yield return new Token(TokenType.Pick, buffer);
                    break;

                case '§':
                    yield return new Token(TokenType.Section, buffer);
                    break;
            }
        }
    }

    private static Token ScanLiteral(TextBuffer buffer)
    {
        buffer.MoveNext();

        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position);

        while (buffer.Peek() is not '"')
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Literal, startPosition, endPosition - startPosition, buffer.Line, buffer.Column);
    }

    private static Token ScanNumber(TextBuffer buffer)
    {
        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position + 1);

        while (buffer.PeekNext(out var nextChar) && (char.IsDigit(nextChar) || nextChar is '.'))
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Number, startPosition, endPosition - startPosition, buffer.Line, buffer.Column);
    }

    private static Token ScanIdentifier(TextBuffer buffer)
    {
        var (startPosition, endPosition) = (Cursor: buffer.Position, buffer.Position + 1);

        while (buffer.PeekNext(out var nextChar) && 
               nextChar is not Pick &&
               (char.IsLetter(nextChar) || char.IsDigit(nextChar)))
        {
            endPosition++;
            buffer.MoveNext();
        }

        return new Token(TokenType.Identifier, startPosition, endPosition - startPosition, buffer.Line, buffer.Column);
    }
}