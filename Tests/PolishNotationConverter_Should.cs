using System.Data;
using FluentAssertions;
using Lab13;
using NUnit.Framework;

namespace Tests
{
    public class PolishNotationConverter_Should
    {
        [Test]
        public void BeAbleToConvertFunctions() =>
            PolishNotationConverter.ConvertPn("x ^ y / (-5 * z) + 10 + func(a, b, c) / 3")
                .Should().Be("+ + / ^ x y * -5 z 10 / func a b c 3");

        [TestCase("1 + 3)")]
        [TestCase("1 + (3 * 2")]
        [TestCase("1 + (3 * 2)))")]
        [TestCase("1 + (3 * (2)))")]
        [TestCase("(((((((((")]
        [TestCase("))()))")]
        public void ThrowOnInvalidBracketSequence(string invalidExpression)
        {
            Assert.Throws<InvalidExpressionException>(() => PolishNotationConverter.ConvertPn(invalidExpression));
        }
        
        #region cases

        [TestCase("5", "5")]
        [TestCase("-5", "-5")]
        [TestCase("9 - 4", "9 4 -")]
        [TestCase("9 - (2 + 2)", "9 2 2 + -")]
        [TestCase("9 - (2 + (1 + 1))", "9 2 1 1 + + -")]
        [TestCase("3 * 3 - (2 + (1 + 1))", "3 3 * 2 1 1 + + -")]
        [TestCase("15 / 5 * 3 - (2 + (1 + 1))", "15 5 / 3 * 2 1 1 + + -")]
        [TestCase("15 / (7 - 2) * 3 - (2 + (1 + 1))", "15 7 2 - / 3 * 2 1 1 + + -")]
        [TestCase("15 / (7 - (1 + 1)) * 3 - (2 + (1 + 1))", "15 7 1 1 + - / 3 * 2 1 1 + + -")]
        [TestCase("3 + 4 * 2 / (1 - 5)^2", "3 4 2 * 1 5 - 2 ^ / +")]
        [TestCase("3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3", "3 4 2 * 1 5 - 2 3 ^ ^ / +")]
        [TestCase("x ^ y / (-5 * z) + 10 + func(a, b, c) / 3", "x y ^ -5 z * / 10 + c b a func 3 / +")]

        #endregion
        public void ConvertToReversedPolishNotationCorrectly(string expression, string expected) =>
            PolishNotationConverter.ConvertRpn(expression).Should().Be(expected);
        
        #region cases

        [TestCase("5", "5")]
        [TestCase("-5", "-5")]
        [TestCase("9 - 4", "- 9 4")]
        [TestCase("9 - (2 + 2)", "- 9 + 2 2")]
        [TestCase("9 - (2 + (1 + 1))", "- 9 + 2 + 1 1")]
        [TestCase("3 * 3 - (2 + (1 + 1))", "- * 3 3 + 2 + 1 1")]
        [TestCase("15 / 5 * 3 - (2 + (1 + 1))", "- * / 15 5 3 + 2 + 1 1")]
        [TestCase("15 / (7 - 2) * 3 - (2 + (1 + 1))", "- * / 15 - 7 2 3 + 2 + 1 1")]
        [TestCase("15 / (7 - (1 + 1)) * 3 - (2 + (1 + 1))", "- * / 15 - 7 + 1 1 3 + 2 + 1 1")]
        [TestCase("3 + 4 * 2 / (1 - 5)^2", "+ 3 / * 4 2 ^ - 1 5 2")]
        [TestCase("3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3", "+ 3 / * 4 2 ^ - 1 5 ^ 2 3")]
        [TestCase("x ^ y / (-5 * z) + 10 + func(a, b, c) / 3", "+ + / ^ x y * -5 z 10 / func a b c 3")]

        #endregion
        public void ConvertToPolishNotationCorrectly(string expression, string expected) =>
            PolishNotationConverter.ConvertPn(expression).Should().Be(expected);
    }
}