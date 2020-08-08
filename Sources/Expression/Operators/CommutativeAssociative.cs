using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Expression
{
    internal class CommutativeAssociativeOperator : Operator
    {
        #region abstract class implementation

        public override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }
        public override IEnumerable<IDvExpr> Operands => Operands_;

        public override Operator CreateInstance(params IDvExpr[] operands)
            => new CommutativeAssociativeOperator(Sign, Priority, OpFunc,
                operands.Length != 0 ? operands : Operands.ToArray());

        #endregion abstract class implementation

        #region subclass functionality

        protected internal HashSet<IDvExpr> Operands_;

        public CommutativeAssociativeOperator(string sign, int prio,
            Func<double[], double> op, params IDvExpr[] operands)
        {
            (Sign, Priority, OpFunc) = (sign, prio, op);
            Operands_ = new HashSet<IDvExpr>(operands);
        }

        #endregion subclass functionality
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