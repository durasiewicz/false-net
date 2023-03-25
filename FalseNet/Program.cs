// See https://aka.ms/new-console-template for more information

using FalseNet.Parser;
using FalseNet.Runtime;

var tokens = Lexer.Lex(@"""

{ Simple addition test }

11111 11111 +

        """);

Parser.Parse(tokens);

Console.WriteLine("Hello, World!");