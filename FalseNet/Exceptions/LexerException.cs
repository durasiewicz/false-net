namespace FalseNet.Exceptions;

public class LexerException : Exception
{
    public int PosX { get; }
    public int PosY { get; }

    public LexerException(int posX, int posY, string message) : base(message)
    {
        PosX = posX;
        PosY = posY;
    }
}