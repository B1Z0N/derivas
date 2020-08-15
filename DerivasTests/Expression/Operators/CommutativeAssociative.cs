using Derivas.Exception;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class CommutativeAssociativeOperatorTests : OperatorsBaseTests
    {
        [Test()]
        public void TestOperators()
        {
            var d = Dict.Add("x", 1).Add("y", 2).Add("z", 3).Get();
            var answers = new Dictionary<IDvExpr, double>()
            {
                { Add("x", "y", "z"), 6 },
                { Add("x", Add("z", "y")), 6 },
                { Mul("x", "y", "z"), 6 },
                { Mul("x", Mul("z", "y")), 6 },
            };

            foreach (var pair in answers)
            {
                Assert.AreEqual(pair.Value, pair.Key.Calculate(d));
            }

            foreach (var op in new Func<object[], IDvExpr>[] { Add, Mul })
            {
                Assert.Throws<DvNotEnoughArgumentsException>(() => op(new object[0]));
                Assert.Throws<DvNotEnoughArgumentsException>(() => op(new object[] { 1 }));
                Assert.Throws<DvNotEnoughArgumentsException>(() => op(new object[] { "x" }));
            }
        }
    }
}