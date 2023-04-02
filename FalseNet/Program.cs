using System.Diagnostics;
using static System.Console;
using FalseNet.Analyzers;
using FalseNet.Exceptions;
using FalseNet.Runtime;

namespace FalseNet;

public static class Program
{
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length != 1)
            {
                WriteLine(
                    $"Usage: {Process.GetCurrentProcess().MainModule?.FileName ??
                              nameof(FalseNet)} <script>.f");
                return;
            }

            var script = File.ReadAllText(args[0]);
            var tokens = Lexer.Lex(script);
            var parser = new Parser();
            parser.Parse(tokens);
        }
        catch (LexerException e)
        {
            WriteLine($"Lexer error: {e.Message}");
        }
        catch (RuntimeException e)
        {
            WriteLine($"Runtime error: {e.Message}");
        }
        catch (Exception e)
        {
            WriteLine(e);
        }
    }
}