using NUnit.Framework;
using System.Collections.Generic;

namespace Derivas.Expression.Tests
{
    using static DvOps;

    [TestFixture]
    public class BinaryOpeartorTests : OperatorsTestUtility
    {
        [Test()]
        public void TestOperators()
        {
            var d = Dict.Add("x", 2).Get();
            var answers = new Dictionary<IDvExpr, double>()
            {
                { Sub("x", 1), 1 },
                { Sub("x", "x"), 0 },
                { Div("x", "x"), 1 },
                { Div("x", 0), double.PositiveInfinity },
                { Pow("x", 2), 4 },
                { Pow(2, "x"), 4 },
                { Pow("x", 0), 1 },
            };

            foreach (var pair in answers)
            {
                Assert.AreEqual(pair.Value, pair.Key.Calculate(d));
            }
        }
    }
}