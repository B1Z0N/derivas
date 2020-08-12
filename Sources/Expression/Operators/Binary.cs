using System;

namespace Derivas.Expression
{
    /// <summary>
    /// Binary ordered operator
    /// </summary>
    internal class BinaryOperator : OrderedOperator
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
            :base(fst, snd)
        {
            (Sign, Priority) = (sign, prio);
            OpFunc = (double[] args) => op(args[0], args[1]);
            BinFunc = op;
        }

        public override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }

        public override Operator CreateInstance(params IDvExpr[] operands)
        {
            if (operands.Length < 2)
            {
                return new BinaryOperator(First, Second, Sign, Priority, BinFunc);
            }
            else
            {
                return new BinaryOperator(operands[0], operands[1], Sign, Priority, BinFunc);
            }
        }
    }

    public static partial class DvOps
    {
        public static IDvExpr Div(object fst, object snd)
            => new BinaryOperator(CheckExpr(fst), CheckExpr(snd), "/", 1, (fst, snd) => fst / snd);

        public static IDvExpr Sub(object fst, object snd)
            => new BinaryOperator(CheckExpr(fst), CheckExpr(snd), "-", 0, (fst, snd) => fst - snd);

        public static IDvExpr Pow(object bas, object pow) => new BinaryOperator(
            CheckExpr(bas), CheckExpr(pow), "^", 2, (bas, pow) => Math.Pow(bas, pow)
        );
    }
}