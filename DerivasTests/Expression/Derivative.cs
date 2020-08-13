using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class DerivativeTests
    {
        private static Dictionary<string, double> EmptyNameVal = new Dictionary<string, double>();
        private static string EmptySymbol = string.Empty;

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
            Assert.AreEqual(Der(Log("x"), "x"), Der(Log("x", DvOps.E), "x"));
        }
    }
}