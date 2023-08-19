namespace FalseNet.Compiler;

public enum TokenType
{
    /// <summary>
    /// { (internal lexer use)
    /// </summary>
    CommentBegin,

    /// <summary>
    /// [
    /// </summary>
    OpenSquareBracket,
    
    /// <summary>
    /// ]
    /// </summary>
    CloseSquareBracket,
        
    /// <summary>
    /// abc
    /// </summary>
    Identifier,

    /// <summary>
    /// 123
    /// </summary>
    Number,
    
    /// <summary>
    /// All characters between double-quotes.
    /// </summary>
    Literal,

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
    Caret,
    
    /// <summary>
    /// ø
    /// </summary>
    Pick,
    
    /// <summary>
    /// §
    /// </summary>
    Section
}