using System.Collections.Generic;
using FluentAssertions;
using Lab13;
using NUnit.Framework;

namespace Tests
{
    public class ExpressionParser_Should
    {
        public static readonly object[] ParserCaseSource =
        {
            new object[] {"1", new List<string>() {"1"}},
            new object[] {"1 + 2", new List<string>() {"1", "+", "2",}},
            new object[] {"1+2", new List<string>() {"1", "+", "2",}},
            new object[] {"-3 + (-5 * 7)", new List<string>() {"-3", "+", "(", "-5", "*", "7", ")"}},
            new object[] {"-x + (-y * 7)", new List<string>() {"-x", "+", "(", "-y", "*", "7", ")"}},
            new object[]
            {
                "-3 + (-5 * 7) / func(x,y,z)",
                new List<string>() {"-3", "+", "(", "-5", "*", "7", ")", "/", "func", "(", "x", "y", "z", ")"}
            },
        };

        [TestCaseSource(nameof(ParserCaseSource))]
        public void ParseExpressionsCorrectly(string expression, List<string> expectedResult)
        {
            ExpressionParser.ParseElements(expression)
                .Should().BeEquivalentTo(expectedResult);
        }
    }
}
