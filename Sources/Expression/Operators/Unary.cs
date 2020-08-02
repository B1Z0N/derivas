using System;
using System.Collections.Generic;
using System.Text;

namespace Derivas.Expression
{
    internal abstract class UnaryOperator : Operator
    {
        public Expr Of
        { 
            get => Operands_[0];
            protected set => Operands_[0] = value;
        }

        protected UnaryOperator(Expr of, string sign, int prio)
        {
            (Of, Sign, Priority) = (of, sign, prio);
            OpFunc = (double[] args) => UnFunc(args[0]);
        }

        protected abstract Func<double, double> UnFunc { get; }

        protected override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }
    }
}
