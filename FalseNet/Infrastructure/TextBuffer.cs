namespace FalseNet.Infrastructure;

internal sealed class TextBuffer
{
    private readonly string _input;

    public TextBuffer(string input)
    {
        _input = input;
        _inputLenght = input.Length;
    }

    private readonly int _inputLenght;
    
    public int Position { get; private set; } = -1;
    public int Line { get; private set; } = 0;
    public int Column { get; private set; } = 0;
    
    public bool MoveNext()
    {
        if (Position + 1 >= _inputLenght)
        {
            return false;
        }

        Position++;

        switch (Peek())
        {
            case '\n':
                Line++;
                Column = 0;
                break;
            
            default:
                Column++;
                break;
        }

        return true;
    }
    
    public char Peek() => _input[Position];

    public bool PeekNext(out char nextChar)
    {
        nextChar = '\0';

        if (Position + 1 >= _inputLenght)
        {
            return false;
        }

        nextChar = _input[Position + 1];
        return true;
    }
}