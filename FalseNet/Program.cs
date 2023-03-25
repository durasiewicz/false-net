// See https://aka.ms/new-console-template for more information

using FalseNet.Parser;

var tokens = Lexer.Lex(@"""

{ Simple addition test }

1 2 +

        """);

Parser.Parse(tokens);

Console.WriteLine("Hello, World!");