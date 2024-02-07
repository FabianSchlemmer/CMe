namespace CMe
{
    public enum TokenType
    {
        EOF,
        Identifier,
        Assignment,

        // Structure
        Semicolon,
        OpenParen,
        CloseParen,
        OpenBraces,
        CloseBraces,
        Comma,

        // Binary Operators
        // Arithmetic
        Plus,
        Minus,
        Times,
        Divide,

        // Logical
        LessThan,
        LessThanOrEqual,
        EqualEqual,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        And,
        Or,
        Not,

        // Unary Operators
        PlusPlus,
        MinusMinus,

        // Literals
        LiteralInt,
        LiteralString,
        LiteralChar,

        // Keywords
        Int,
        Char,
        Void,
        Float,
        Double,
        Struct,
        Enum,
        TypeDef,
        Return,
        For,
        If,
        Else,
    }
}
