namespace FalseNet.Compiler;

public readonly record struct Token(TokenType TokenType,
    int Start,
    int Lenght = 1);