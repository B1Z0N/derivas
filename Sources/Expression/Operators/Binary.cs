using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Exception
{
    internal class DvZeroDivisionException : DvBaseException
    {
        public DvZeroDivisionException(Expr quotient)
            : base($"You can't divide anything by zero, particularly {quotient.Represent()}")
        {
        }
    }
}

namespace Derivas.Expression
{
    internal abstract class BinaryOperator : Operator
    {
        public Expr First
        {
            get => Operands_[0];
            protected set => Operands_[0] = value;
        }

        public Expr Second
        {
            get => Operands_[1];
            protected set => Operands_[1] = value;
        }

        protected BinaryOperator(Expr fst, Expr snd, string sign, int prio)
        {
            (First, Second, Sign, Priority) = (fst, snd, sign, prio);
            OpFunc = (double[] args) => BinFunc(args[0], args[1]);
        }

        protected abstract Func<double, double, double> BinFunc { get; }
        protected override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }
    }

    internal class Division : BinaryOperator
    {
        protected override Func<double, double, double> BinFunc { get; }

        public Division(Expr fst, Expr snd) : base(fst, snd, "/", 1) 
        {
            BinFunc = (fst, snd) => snd != 0d ? fst / snd : 
                throw new DvZeroDivisionException(Second);
        }
    }
}
