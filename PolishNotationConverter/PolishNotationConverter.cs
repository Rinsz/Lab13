using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab13
{
    public static class PolishNotationConverter
    {
        private static readonly string numberRegexPattern = "[-]?(\\d+|\\w{1})";

        public static string ConvertRpn(string expression) => string.Join(' ', Convert(expression));

        public static string ConvertPn(string expression) => ConvertPostfixToPrefix(Convert(expression));

        private static string ConvertPostfixToPrefix(IEnumerable<string> rpnElements)
        {
            var operands = new Stack<string>();
            var sb = new StringBuilder();
            
            foreach (var element in rpnElements)
            {
                if (Regex.Matches(element, numberRegexPattern).Count == 1)
                {
                    operands.Push(element);
                    continue;
                }

                if (element[0].IsOperator())
                {
                    var first = operands.Pop();
                    sb.AppendJoin(' ', element, operands.Pop(), first);
                    operands.Push(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(element + ' ');
                    var funcArgs = new List<string>();
                    while (operands.Count > 0 && !operands.Peek()[0].IsOperator())
                        funcArgs.Add(operands.Pop());
                    sb.AppendJoin(' ', funcArgs);
                    operands.Push(sb.ToString());
                    sb.Clear();
                }
            }

            if (operands.Count > 1)
                throw new Exception("wha?");

            return operands.Pop();
        }
        
        private static IEnumerable<string> Convert(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new Exception("expression can't be empty");
            CheckBracketSequence(expression);

            var expressionElements = ExpressionParser.ParseElements(expression.Replace(" ", "")).ToList();
            var operators = new Stack<string>();
            var polishNotationElements = new List<string>();

            for (var i = 0; i < expressionElements.Count; i++)
            {
                if (Regex.IsMatch(expressionElements[i], numberRegexPattern)
                && (i + 1 >= expressionElements.Count || i + 1 < expressionElements.Count && expressionElements[i + 1] != "("))
                {
                    polishNotationElements.Add(expressionElements[i]);
                    continue;
                }

                if (expressionElements[i].Length > 1)
                {
                    operators.Push(expressionElements[i]);
                    if (expressionElements[++i] != "(")
                        throw new ArithmeticException("");
                    i++;

                    var funcArguments = new List<string>();
                    while (i < expressionElements.Count && expressionElements[i] != ")")
                        funcArguments.Add(expressionElements[i++]);
                    funcArguments.Reverse();
                    
                    polishNotationElements.AddRange(funcArguments);
                    continue;
                }

                if (expressionElements[i][0].IsOperator() && !expressionElements[i][0].IsBracket())
                {
                    while (operators.Count > 0 
                        && GetElementPriority(operators.Peek()) >= GetElementPriority(expressionElements[i])
                        && expressionElements[i] != "^")
                        polishNotationElements.Add(operators.Pop());
                    operators.Push(expressionElements[i]);
                }
                
                switch (expressionElements[i])
                {
                    case "(":
                        operators.Push(expressionElements[i]);
                        break;
                    case ")":
                    {
                        while (operators.Peek() != "(")
                            polishNotationElements.Add(operators.Pop());
                        operators.Pop();
                        break;
                    }
                }
            }
            
            if(operators.Count > 0)
                polishNotationElements.AddRange(operators);

            return polishNotationElements.Where(e => e != "(" && e != ")");
        }

        private static void CheckBracketSequence(string expression)
        {
            var openedBrackets = new Stack<char>();
            foreach (var symbol in expression)
            {
                if(symbol == '(')
                    openedBrackets.Push(symbol);
                if (symbol == ')')
                {
                    if (openedBrackets.Count == 0)
                        throw new InvalidExpressionException("Invalid bracket sequence");
                    openedBrackets.Pop();
                }
            }

            if (openedBrackets.Count > 0)
                throw new InvalidExpressionException("Invalid bracket sequence");
        }
        
        private static int GetElementPriority(string elem) =>
            elem switch
            {
                "(" => 0,
                ")" => 0,
                "+" => 1,
                "-" => 1,
                "*" => 2,
                "/" => 2,
                "^" => 3,
                _ => 4
            };

    }
}
