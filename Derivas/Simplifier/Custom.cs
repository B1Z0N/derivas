using Derivas.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Simplifier
{
    internal sealed class CustomSimplifier : BaseSimplifier
    {
        public CloneableExpr From { get; }
        public CloneableExpr To { get; }

        public CustomSimplifier(CloneableExpr from, CloneableExpr to)
        {
            (From, To) = (from, to);
        }

        protected override IDvExpr Get(OrderedOperator expr)
        {
            expr = base.Get(expr) as OrderedOperator;

            return expr.CreateInstance(
                (from op in expr.Operands.Select(Simplify)
                 select From.Equals(op) ? To : op)
                .ToArray()
            );
        }

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            expr = base.Get(expr) as CommutativeAssociativeOperator;
            Func<CloneableExpr, IEnumerable<CloneableExpr>> lst =
                el => new List<CloneableExpr>() { el };

            return From is CommutativeAssociativeOperator from && from.Sign == expr.Sign ?
                expr.ReplaceSubOperands(from.Operands, lst(To)) :
                expr.ReplaceSubOperands(lst(From), lst(To));
        }
    }

    public sealed partial class DvSimplifier
    {
        public DvSimplifier ByCustom(object from, object to)
        {
            InvocationQ.Enqueue(
                new CustomSimplifier(
                    Utils.CheckExpr(from),
                    Utils.CheckExpr(to)
                )
            );
            return this;
        }

        public DvSimplifier ByCustom(Func<IDvExpr, IDvExpr> simplF)
        {
            InvocationQ.Enqueue(new FuncSimplifier(simplF));
            return this;
        }
    }
}