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

        public override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }

        public override MultiArgOperator CreateInstance(params IDvExpr[] operands) 
            => new CommutativeAssociativeOperator(Sign, Priority, OpFunc, 
                operands.Length != 0 ? operands : Operands_.ToArray());
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