// See https://aka.ms/new-console-template for more information

using FalseNet.Analyzers;
using FalseNet.Runtime;

var p = @"

{ Simple addition test 

2 3 * 1 + 2 * 5 - 3 / q:

2 2 * z:

q;z;+



[1+]f:

1f;!f;!f;!

""The result is: "" . 
}

1[$10>~][1+]#.

        ";

var tokens = Lexer.Lex(p);
var parser = new Parser();
parser.Parse(tokens);

Console.WriteLine("Hello, World!");