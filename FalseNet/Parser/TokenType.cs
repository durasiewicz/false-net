namespace FalseNet.Parser;

public enum TokenType
{
    CommentBegin,
    CommentEnd,
    FunctionBegin,
    FunctionEnd,
    Variable,
    Character,
    Number,
    ValueAssigment,
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
    Condition
}