namespace FalseNet.Exceptions;

public class LexerException : Exception
{
    public int Column { get; }
    public int Line { get; }

    public LexerException(int line, int column, string message) : base(message)
    {
        Column = column;
        Line = line;
    }
}