using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CMe
{
    public class Token(TokenType type, object? value = null)
    {
        public TokenType Type { get; private set; } = type;
        public object? Value { get; private set; } = value;

        public T ValueAs<T>()
        {
            Debug.Assert(Value != null);
            return (T)Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Token))
                return false;

            var objAsTok = (Token)obj;
            if (Type != objAsTok.Type)
                return false;
            if (Value is null)
                return objAsTok.Value is null;
            if (!Value.Equals(objAsTok.Value))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }

        public override string ToString()
        {
            return $"TokenType: {Type}; Value: {Value}.";
        }
    }
}
