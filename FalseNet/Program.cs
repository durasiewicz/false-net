// See https://aka.ms/new-console-template for more information

using FalseNet.Analyzers;
using FalseNet.Runtime;

var p = @"

{ Simple addition test }

"" ala ma kota ""


2 3 * 1 + 2 * 5 - 3 / q:

2 2 * z:

q;z;+

        ";

var tokens = Lexer.Lex(p);

Parser.Parse(tokens);

Console.WriteLine("Hello, World!");