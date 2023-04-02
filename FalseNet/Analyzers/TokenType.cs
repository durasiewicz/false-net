namespace FalseNet.Analyzers;

public enum TokenType
{
    /// <summary>
    /// { (internal lexer use)
    /// </summary>
    CommentBegin,

    /// <summary>
    /// [
    /// </summary>
    OpenBracket,

    /// <summary>
    /// ]
    /// </summary>
    CloseBracket,

    /// <summary>
    /// abc
    /// </summary>
    StringLiteral,

    /// <summary>
    /// 123
    /// </summary>
    NumericLiteral,

    /// <summary>
    /// :
    /// </summary>
    Colon,

    /// <summary>
    /// ;
    /// </summary>
    Semicolon,

    /// <summary>
    /// +
    /// </summary>
    Plus,

    /// <summary>
    /// -
    /// </summary>
    Minus,

    /// <summary>
    /// *
    /// </summary>
    Asterisk,

    /// <summary>
    /// /
    /// </summary>
    Slash,

    /// <summary>
    /// =
    /// </summary>
    Equals,

    /// <summary>
    /// >
    /// </summary>
    GreaterThan,

    /// <summary>
    /// &
    /// </summary>
    Ampersand,

    /// <summary>
    /// |
    /// </summary>
    Bar,

    /// <summary>
    /// $
    /// </summary>
    Dollar,

    /// <summary>
    /// %
    /// </summary>
    Percent,

    /// <summary>
    /// \
    /// </summary>
    Backslash,

    /// <summary>
    /// @
    /// </summary>
    At,

    /// <summary>
    /// ?
    /// </summary>
    Question,

    /// <summary>
    /// !
    /// </summary>
    Exclamation,

    /// <summary>
    /// ~
    /// </summary>
    Tilde,

    /// <summary>
    /// "abc"
    /// </summary>
    DoubleQuotedStringLiteral,

    /// <summary>
    /// .
    /// </summary>
    Dot,

    /// <summary>
    /// ,
    /// </summary>
    Comma,

    /// <summary>
    /// _
    /// </summary>
    Underscore,

    /// <summary>
    /// #
    /// </summary>
    Hash,
    
    /// <summary>
    /// ^
    /// </summary>
    Caret
}