namespace FalseNet.Analyzers;

public record Token(int Position,
    int Line,
    TokenType Type,
    string? Value = null);