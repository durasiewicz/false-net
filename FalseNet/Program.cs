using FalseNet.Analyzers;
using FalseNet.Runtime;

var p = @"

""Please enter your name: "" 0[$10=~][^]#%
""Hello: "" [$0>][,]#

";

var tokens = Lexer.Lex(p);
var parser = new Parser();
parser.Parse(tokens);

Console.WriteLine("Hello, World!");