using System.Collections.Generic;

namespace Lab13
{
    public static class CharExtensions
    {
        private static readonly HashSet<char> Operators = new() { '+', '-', '/', '*', '^' };

        public static bool IsOperator(this char c) => Operators.Contains(c);

        public static bool IsBracket(this char c) => c == '(' || c == ')';
        
        public static bool IsOperatorOrBracket(this char c) => c.IsOperator() || c.IsBracket();
    }
}