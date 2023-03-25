namespace FalseNet.Parser;

public record Token(int Position,
    int Line,
    TokenType Type,
    string? Value = null);