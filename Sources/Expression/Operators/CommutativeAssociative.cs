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

        protected internal List<IDvExpr> Operands_;

        public CommutativeAssociativeOperator(string sign, int prio,
            Func<double[], double> op, params IDvExpr[] operands)
        {
            (Sign, Priority, OpFunc) = (sign, prio, op);
            Operands_ = new List<IDvExpr>(operands);
        }

        public CommutativeAssociativeOperator ReplaceSubOperands(
            IEnumerable<IDvExpr> replaceOperands,
            IEnumerable<IDvExpr> with)
        {
            if (replaceOperands.Count() == 0)
            {
                return this;
            }

            if (replaceOperands.Count() == 1)
            {
                var replace = replaceOperands.First();

                IEnumerable<IDvExpr> res = new List<IDvExpr>();
                foreach (var operand in Operands)
                {
                    if (replace.Equals(operand))
                    {
                        res = res.Concat(with);
                    }
                    else
                    {
                        res.Append(operand);
                    }
                }

                return CreateInstance(res.ToArray()) as CommutativeAssociativeOperator;
            }

            var counter = new Dictionary<IDvExpr, int>();
            foreach (var operand in Operands)
            {
                if (counter.ContainsKey(operand)) counter[operand] += 1;
                else counter[operand] = 1;
            }

            int replaceCount = Operands.Count();
            foreach (var operand in replaceOperands)
            {
                if (!counter.ContainsKey(operand)) counter[operand] = 0;
                replaceCount = Math.Min(replaceCount, counter[operand]);
            }

            IEnumerable<IDvExpr> 
                replaced = Enumerable.Repeat(with, replaceCount).SelectMany(x => x),
                untouched = new List<IDvExpr>(Operands.Except(replaceOperands));
            foreach (var operand in replaceOperands)
            {
                counter[operand] -= replaceCount;
                untouched = untouched.Concat(Enumerable.Repeat(operand, counter[operand]));
            }

            return CreateInstance(
                Enumerable.Concat(untouched, replaced).ToArray()
            ) as CommutativeAssociativeOperator;
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
            args => args.Aggregate(1d, (fst, snd) => fst * snd);

        public static IDvExpr Mul(params IDvExpr[] args)
            => new CommutativeAssociativeOperator("*", 1, Multiplication, args);
    }
}