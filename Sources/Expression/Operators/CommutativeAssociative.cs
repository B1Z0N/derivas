using System;
using System.Linq;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Expression
{
    internal class CommutativeAssociativeOperator : MultiArgOperator
    {
        public CommutativeAssociativeOperator(string sign, int prio, 
            Func<double[], double> op, params IDvExpr[] operands)
            : base(operands) => (Sign, Priority, OpFunc) = (sign, prio, op);

        protected override Func<double[], double> OpFunc { get; }
        protected override int Priority { get; }
        protected override string Sign { get; }

        public override IDvExpr Clone() 
            => new CommutativeAssociativeOperator(Sign, Priority, OpFunc, Operands_.ToArray());
    }

    public static partial class DvOps
    {
        private static Func<double[], double> Addition =
            args => args.Aggregate(0d, (fst, snd) => fst + snd);
        public static IDvExpr Add(params IDvExpr[] args) 
            => new CommutativeAssociativeOperator("+", 0, Addition, args);
        
        private static Func<double[], double> Multiplication =
            args => args.Aggregate(0d, (fst, snd) => fst * snd);
        public static IDvExpr Mul(params IDvExpr[] args) 
            => new CommutativeAssociativeOperator("*", 1, Multiplication, args);
    }
}