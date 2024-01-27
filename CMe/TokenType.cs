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

        // Binary Operators
        // Arithmetic
        Plus,
        Times,
        // Logical
        LessThan,

        // Unary Operators
        PlusPlus,

        // Literals
        LiteralInt,
        LiteralString,
        LiteralChar,

        // Keywords
        Int,
        Return,
        For,
    }
}
