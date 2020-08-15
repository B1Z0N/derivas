using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class DerivativeTests : BaseTests
    {
        [Test()]
        public void TestConstant()
        {
            foreach (var @const in new double[] {
                1, -1, 0, double.NaN, double.NegativeInfinity,
                double.PositiveInfinity, double.Epsilon})
            {
                Assert.AreEqual(
                    Der(@const, EmptySymbol).Calculate(EmptyNameVal), 0d
                );
            }
        }

        [Test()]
        public void TestSymbol()
        {
            var symbols = new string[] { "x", "y", "x" }.Select(Sym);

            foreach (var sym1 in symbols)
            {
                foreach (var sym2 in symbols)
                {
                    Assert.AreEqual(
                        Der(sym1, sym2).Calculate(EmptyNameVal), sym1.Equals(sym2) ? 1d : 0d
                    );
                }
            }
        }

        [Test()]
        public void TestLogarithm()
        {
            var d = new Dictionary<string, double>() { { "x", 1 } };


            Assert.AreEqual(Der(Log(1), "x").Calculate(EmptyNameVal), 0d);
            Assert.AreEqual(Der(Log("x"), "x").Calculate(d), 1d);
            Assert.AreEqual(Der(Log("x"), "x"), Der(Log("x", DvConsts.E), "x"));
        }

        [Test()]
        public void TestUnaryOperator()
        {
            var answers = new Dictionary<IDvExpr, IDvExpr>()
            {
                { Cos("x"), Mul(Sin("x"), -1) },
                { Sin("x"), Cos("x") },
                { Tan("x"), Div(1, Pow(Cos("x"), 2)) },
                { Cotan("x"), Mul(-1, Div(1, Pow(Sin("x"), 2))) },
            };

            var d = new Dictionary<string, double>() { { "x", 1 } };

            foreach (var pair in answers.AsEnumerable())
            {
                Assert.AreEqual(
                    Der(pair.Key, "x").Calculate(d),
                    pair.Value.Calculate(d)
                );
            }

        }

        [Test()]
        public void TestCommutativeAssociativeOperator()
        {
            var answers = new Dictionary<IDvExpr, IDvExpr>()
            {
                { Add("x", "y"), Const(1) },
                { Mul("x", 1, "y"), Sym("y") },
            };

            var d = new Dictionary<string, double>() { { "x", 1 }, { "y", 5 } };

            foreach (var pair in answers.AsEnumerable())
            {
                Assert.AreEqual(
                    Der(pair.Key, "x").Calculate(d),
                    pair.Value.Calculate(d)
                );
            }
        }

        [Test()]
        public void TestBinaryOperator()
        {
            var answers = new Dictionary<IDvExpr, IDvExpr>()
            {
                { Div("x", "y"),  Div(1, "y") },
                { Div(Pow("x", 2), "x"),  Const(1) },
                { Sub(1, "x"), Mul(-1, "x") },
                { Pow("x", 2), Mul(2, "x") },
                { Pow("x", "x"), Mul(Pow("x", "x"), Add(1, Log("x"))) },
            };

            var d = new Dictionary<string, double>() { { "x", 1 }, { "y", 5 } };

            foreach (var pair in answers.AsEnumerable())
            {
                Assert.AreEqual(
                    Der(pair.Key, "x").Calculate(d),
                    pair.Value.Calculate(d)
                );
            }
        }

        public class UnknownExpr : IDvExpr
        {
            public double Calculate(IDictionary<string, double> concrete)
            {
                throw new System.NotImplementedException();
            }

            public bool Equals([AllowNull] IDvExpr other)
            {
                throw new System.NotImplementedException();
            }

            public string Represent()
            {
                throw new System.NotImplementedException();
            }
        }

        [Test()]
        public void TestUnknownExpr()
        {
            Assert.Throws<DvDerivativeMismatchException>(() => Der(new UnknownExpr(), "x"));
        }
    }
}