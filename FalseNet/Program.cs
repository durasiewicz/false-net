using FalseNet.Analyzers;
using FalseNet.Runtime;

var p = @"

{ [$1=$[\%1\]?~[$1-f;!*]?]f:

6f;!. }

 [1+$10>~[f;!]?]f:

2f;!. 

{ 1_[""Work""]? }

        ";

var tokens = Lexer.Lex(p);
var parser = new Parser();
parser.Parse(tokens);

Console.WriteLine("Hello, World!");