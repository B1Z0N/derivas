using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Expression
{
    internal class BinaryOperator : MultiArgOperator
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

        public BinaryOperator(
            Expr fst, Expr snd, string sign, int prio,
            Func<double, double, double> op)
        {
            (First, Second, Sign, Priority) = (fst, snd, sign, prio);
            OpFunc = (double[] args) => op(args[0], args[1]);
        }

        protected override Func<double[], double> OpFunc { get; }
        protected override int Priority { get; }
        protected override string Sign { get; }
    }

    internal static partial class OperatorCollection
    {
        public static Expr Division(Expr fst, Expr snd) 
            => new BinaryOperator(fst, snd, "/", 1, (fst, snd) => fst / snd);

        public static Expr Subtraction(Expr fst, Expr snd)
            => new BinaryOperator(fst, snd, "-", 0, (fst, snd) => fst - snd);

        public static Expr Power(Expr bas, Expr pow) => new BinaryOperator(
            bas, pow, "^", 2, (bas, pow) => Math.Pow(bas, pow)
        );
    }
}
