using System;

namespace Derivas.Expression
{
    internal class BinaryOperator : MultiArgOperator
    {
        public IDvExpr First
        {
            get => Operands_[0];
            protected set => Operands_[0] = value;
        }

        public IDvExpr Second
        {
            get => Operands_[1];
            protected set => Operands_[1] = value;
        }

        private Func<double, double, double> BinFunc { get; }

        public BinaryOperator(
             IDvExpr fst, IDvExpr snd, string sign, int prio,
            Func<double, double, double> op)
        {
            (First, Second, Sign, Priority) = (fst, snd, sign, prio);
            OpFunc = (double[] args) => op(args[0], args[1]);
            BinFunc = op;
        }

        protected override Func<double[], double> OpFunc { get; }
        protected override int Priority { get; }
        protected override string Sign { get; }

        public override IDvExpr Clone() 
            => new BinaryOperator(First, Second, Sign, Priority, BinFunc);
    }

    public static partial class DvOps
    {
        public static IDvExpr Div(IDvExpr fst, IDvExpr snd)
            => new BinaryOperator(fst, snd, "/", 1, (fst, snd) => fst / snd);

        public static IDvExpr Sub(IDvExpr fst, IDvExpr snd)
            => new BinaryOperator(fst, snd, "-", 0, (fst, snd) => fst - snd);

        public static IDvExpr Pow(IDvExpr bas, IDvExpr pow) => new BinaryOperator(
            bas, pow, "^", 2, (bas, pow) => Math.Pow(bas, pow)
        );
    }
}