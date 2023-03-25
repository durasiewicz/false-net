// See https://aka.ms/new-console-template for more information

using FalseNet.Parser;

Lexer.Lex(@"""

{ This is comment with { nested comment } supported }

{
        """);

Console.WriteLine("Hello, World!");