using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Derivas.Expression.Tests
{
    using static DvOps;

    [TestFixture]
    public class ExoticOperatorTests : OperatorsTestUtility
    {
        [Test()]
        public void TestLog()
        {
            var d = Dict.Add("x", 1).Get();
            var answers = new Dictionary<IDvExpr, double>
            {
                { Log(3), Math.Log(3) },
                { Log("x", "x"), double.NaN },
                { Log(3, "x"), double.NaN },
                { Log("x", 5), 0d },
            };

            foreach (var pair in answers)
            {
                Assert.AreEqual(pair.Value, pair.Key.Calculate(d));
            }
        }
    }
}