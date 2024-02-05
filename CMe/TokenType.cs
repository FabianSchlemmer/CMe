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
        Times,

        // Logical
        LessThan,
        EqualEqual,

        // Unary Operators
        PlusPlus,

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
    }
}
