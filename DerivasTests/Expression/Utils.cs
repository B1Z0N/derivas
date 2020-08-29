using NUnit.Framework;
using System;
using System.Collections.Generic;

using Derivas.Expression;
using Derivas.Exception;
using System.Globalization;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class UtilsTests : TestUtility
    {
        [Test()]
        public void TestConstants()
        {
            Assert.AreEqual(Math.E, DvOps.DvConsts.E.Calculate(EmptyNameVal));
            Assert.AreEqual(Math.PI, DvOps.DvConsts.PI.Calculate(EmptyNameVal));
        }

        [Test()]
        public void TestDict()
        {
            Assert.AreEqual(
                DvOps.Dict.Add("x", 1).Add("y", 3).Get(), 
                new Dictionary<string, double>() { { "x", 1 }, { "y", 3 } }
            );
        }

        [Test()]
        public void TestCheckExpr()
        {
            foreach (var num in new object[] { 1, 1f, 1d })
            {
                var @double = (num as IConvertible).ToDouble(new CultureInfo("en-US"));
                Assert.AreEqual(new Constant(@double), Utils.CheckExpr(num));
            }

            Assert.AreEqual(new Symbol("x"), Utils.CheckExpr("x"));
            Assert.Throws<DvExpressionMismatchException>(() => Utils.CheckExpr(true));
        }
    }
}