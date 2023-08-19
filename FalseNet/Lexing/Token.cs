using FalseNet.Infrastructure;

namespace FalseNet.Lexing;

internal record Token(TokenType Type,
    int Start,
    int Lenght,
    int Line,
    int Column,
    string? Value = null)
{
    public Token(TokenType type, TextBuffer buffer)
        : this(type, buffer.Position, 1, buffer.Line, buffer.Column)
    {
    }
}