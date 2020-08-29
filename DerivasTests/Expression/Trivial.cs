using Derivas.Exception;
using NUnit.Framework;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression.Tests
{
    [TestFixture]
    public class TrivialTests : TestUtility
    {
        [Test()]
        public void TestConstant()
        {
            foreach (var @const in new double[] {
                1, -1, 0, double.NaN, double.NegativeInfinity,
                double.PositiveInfinity, double.Epsilon})
            {
                Assert.AreEqual(
                    Const(@const).Calculate(EmptyNameVal), @const
                );
            }
        }

        [Test()]
        public void TestSymbol()
        {
            Assert.AreEqual(Sym("x").Calculate(Dict.Add("x", 1).Get()), 1);
            Assert.Throws<DvSymbolMismatchException>(() => Sym("x").Calculate(Dict.Get()));
        }
    }
}