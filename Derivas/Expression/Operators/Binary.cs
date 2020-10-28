using System;

namespace Derivas.Expression
{
    /// <summary>
    /// Binary ordered operator
    /// </summary>
    internal class BinaryOperator : OrderedOperator
    {
        public CloneableExpr First
        {
            get => Operands_[0];
            protected set => Operands_[0] = value;
        }

        public CloneableExpr Second
        {
            get => Operands_[1];
            protected set => Operands_[1] = value;
        }

        private Func<double, double, double> BinFunc { get; }

        public BinaryOperator(
             CloneableExpr first, CloneableExpr second, string sign, int prio,
            Func<double, double, double> op)
            :base(first, second)
        {
            (Sign, Priority) = (sign, prio);
            OpFunc = (double[] args) => op(args[0], args[1]);
            BinFunc = op;
        }

        public override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] operands)
        {
            if (operands.Length < 2)
            {
                return new BinaryOperator(
                    First.CreateInstance(), Second.CreateInstance(), Sign, Priority, BinFunc);
            }
            else
            {
                return new BinaryOperator(operands[0].CreateInstance(), operands[1].CreateInstance(), Sign, Priority, BinFunc);
            }
        }
    }
}