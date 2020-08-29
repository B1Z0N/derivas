using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    
    public class TestUtility
    {
        public static Dictionary<string, double> EmptyNameVal = new Dictionary<string, double>();
        public static string EmptySymbol = string.Empty;
    }

    [TestFixture]
    public class BaseTests
    {
        private class CustomExpr : IDvExpr
        {
            public double Calculate(IDictionary<string, double> concrete)
            {
                throw new NotImplementedException();
            }

            public bool Equals([AllowNull] IDvExpr other)
            {
                throw new NotImplementedException();
            }

            public string Represent()
            {
                throw new NotImplementedException();
            }
        }

        [Test()]
        public void TestCloneableWrapper()
        {
            var expr = new CustomExpr();
            var wrapped = new CloneableExpr.Wrapper(expr);

            Assert.Throws<NotImplementedException>(
                () => wrapped.Calculate(TestUtility.EmptyNameVal)
            );

            Assert.Throws<NotImplementedException>(
                () => wrapped.Represent()
            );

            Assert.AreNotSame(wrapped, wrapped.CreateInstance());
        }

        [Test()]
        public void TestCloneable()
        {
            var expressions = new IDvExpr[]
            {
                Add(1, 2),
                Add(Mul(1, 2), 3),
                Log(Add(1, Cos(Sub(Div(1, 2), Mul(5, 4, 3, 2, "x")))))
            }.Select(el => el as CloneableExpr);

            foreach (var expr in expressions)
            {
                var copy = expr.CreateInstance();
                Assert.AreEqual(expr, copy);
                Assert.AreNotSame(expr, copy);

                var simplified = Simpl(expr).ByConst().ByPolynom().Simplify();

                Assert.AreEqual(expr, copy);
                Assert.AreNotEqual(expr, simplified);
                Assert.AreNotSame(expr, simplified);
            }
        }
    }
}