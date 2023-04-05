namespace FalseNet.Exceptions;

public class LexerException : Exception
{
    public int Position { get; }
    public int Line { get; }

    public LexerException(int position, int line, string message) : base(message)
    {
        Position = position;
        Line = line;
    }
}