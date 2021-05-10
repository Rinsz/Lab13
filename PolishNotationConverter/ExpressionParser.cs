using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("Tests")]

namespace Lab13
{
    internal static class ExpressionParser
    {
        internal static IEnumerable<string> ParseElements(string expression)
        {
            var result = new List<string>();
            var sb = new StringBuilder();

            for (var i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '-' && (i - 1 < 0 || expression[i - 1].IsOperatorOrBracket()))
                    sb.Append(expression[i++]);
                
                while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
                    sb.Append(expression[i++]);

                if (sb.Length > 0)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                
                if (i >= expression.Length)
                    break;
                
                if (expression[i].IsOperatorOrBracket())
                    result.Add(expression[i].ToString());
            }

            return result;
        }
    }
}