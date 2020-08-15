using Derivas.Exception;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class UnaryOperatorTests : OperatorsBaseTests
    {
        [Test()]
        public void TestOperators()
        {
            var d = Dict.Add("x", 1).Get();
            var answers = new Dictionary<IDvExpr, double>
            {
                { Cos(Math.PI), -1 },
                { Sin(Math.PI), 0 },
                { Tan(Math.PI), 0 },
                { Cotan(Math.PI), 1 / Math.Tan(Math.PI) },
            };

            foreach (var pair in answers)
            {
                Assert.AreEqual(pair.Value, pair.Key.Calculate(d), 10);
            }
        }
    }
}