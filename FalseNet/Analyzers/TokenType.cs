namespace FalseNet.Analyzers;

public enum TokenType
{
    CommentBegin,
    CommentEnd,
    FunctionBegin,
    FunctionEnd,
    Variable,
    Character,
    Number,
    ValueSet,
    ValueFetch,
    Addition,
    Substraction,
    Multiplication,
    Division,
    Minus,
    Equals,
    GreaterThan,
    And,
    Or,
    Not,
    Duplicate,
    Delete,
    Swap,
    Rotate,
    Pick,
    Condition,
    Print,
    FunctionCall,
    Negation,
    Literal
}