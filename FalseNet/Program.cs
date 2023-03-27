// See https://aka.ms/new-console-template for more information

using FalseNet.Analyzers;
using FalseNet.Runtime;

var tokens = Lexer.Lex(@"

{ Simple addition test }

"" ala ma kota ""

2 3 * 1 + 2 * 5 - 3 /

        ");

Parser.Parse(tokens);

Console.WriteLine("Hello, World!");