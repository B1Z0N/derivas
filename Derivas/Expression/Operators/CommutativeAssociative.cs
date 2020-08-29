using Derivas.Exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Exception
{
    /// <summary>
    /// Wrong number of arguments passed
    /// </summary>
    public class DvNotEnoughArgumentsException : DvBaseException
    {
        public int NOrMore;
        public DvNotEnoughArgumentsException(int nOrMore)
            : base($"Not enough arguments passed, accepts {nOrMore} or more")
            => NOrMore = nOrMore;
    }
}

namespace Derivas.Expression
{
    internal class CommutativeAssociativeOperator : Operator
    {
        #region abstract class implementation

        public override Func<double[], double> OpFunc { get; }
        public override int Priority { get; }
        public override string Sign { get; }
        public override IEnumerable<CloneableExpr> Operands => Operands_;

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] operands)
            => new CommutativeAssociativeOperator(Sign, Priority, OpFunc,
                operands.Length != 0 ? operands : Operands.ToArray());

        #endregion abstract class implementation

        #region subclass functionality

        protected internal List<CloneableExpr> Operands_;

        public CommutativeAssociativeOperator(string sign, int prio,
            Func<double[], double> op, params CloneableExpr[] operands)
        {
            (Sign, Priority, OpFunc) = (sign, prio, op);
            Operands_ = new List<CloneableExpr>(FlattenSubOperands(operands));
        }

        public bool IsSameType(CloneableExpr to)
            => to is CommutativeAssociativeOperator op && op.Sign == Sign;


        /// <summary>
        /// Transform Caop(1, 2, Caop(1, 2, 3)) to Caop(1, 2, 3, 4)
        /// </summary>
        private IEnumerable<CloneableExpr> FlattenSubOperands(IEnumerable<CloneableExpr> operands)
        {
            IEnumerable<CloneableExpr> res = new List<CloneableExpr>();
            foreach (var operand in operands)
            {
                if (operand is CommutativeAssociativeOperator op && op.Sign == Sign)
                {
                    res = res.Concat(FlattenSubOperands(op.Operands));
                }
                else
                {
                    res = res.Append(operand);
                }
            }

            return res;
        }

        /// <summary>
        /// Replace some operand/operands with other operands
        /// </summary>
        public CloneableExpr ReplaceSubOperands(
            IEnumerable<CloneableExpr> replaceOperands,
            IEnumerable<CloneableExpr> with)
        {
            if (replaceOperands.Count() == 0)
            {
                return this;
            }

            if (replaceOperands.Count() == 1)
            {
                var replace = replaceOperands.First();

                IEnumerable<CloneableExpr> res = new List<CloneableExpr>();
                foreach (var operand in Operands)
                {
                    if (replace.Equals(operand))
                    {
                        res = res.Concat(with);
                    }
                    else
                    {
                        res = res.Append(operand);
                    }
                }

                return CreateInstance(res.ToArray()) as CommutativeAssociativeOperator;
            }

            var counter = new Dictionary<CloneableExpr, int>();
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

            IEnumerable<CloneableExpr>
                replaced = Enumerable.Repeat(with, replaceCount).SelectMany(x => x),
                untouched = new List<CloneableExpr>(Operands.Except(replaceOperands));
            foreach (var operand in replaceOperands)
            {
                counter[operand] -= replaceCount;
                untouched = untouched.Concat(Enumerable.Repeat(operand, counter[operand]));
            }

            var instance = CreateInstance(
                Enumerable.Concat(untouched, replaced).ToArray()
            ) as CommutativeAssociativeOperator;

            return instance.Operands.Count() == 1 ? instance.Operands_[0] : instance;
        }

        #endregion subclass functionality

        #region equals related stuff

        public override bool Equals(IDvExpr expr)
            => expr is CommutativeAssociativeOperator @operator && 
            Sign == @operator.Sign && new HashSet<IDvExpr>(Operands)
            .SetEquals(@operator.Operands);

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Sign);

        #endregion
    }

    public static partial class DvOps
    {
        #region helpers

        private static Func<double[], double> Addition =
            args => args.Aggregate(0d, (fst, snd) => fst + snd);

        private static Func<double[], double> Multiplication =
            args => args.Aggregate(1d, (fst, snd) => fst * snd);

        private static IDvExpr CheckForLessThanTwo(Func<CloneableExpr[], CloneableExpr> createF, params CloneableExpr[] args)
            => args.Count() < 2 ? throw new DvNotEnoughArgumentsException(2) : createF(args);

        #endregion helpers

        #region userspace methods

        public static IDvExpr Add(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator("+", 0, Addition, ops),
                CheckExpr(args)
            );

        public static IDvExpr Mul(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator("*", 1, Multiplication, ops),
                CheckExpr(args)
            );

        #endregion userspace methods
    }
}