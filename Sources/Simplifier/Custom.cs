using Derivas.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Simplifier
{
    internal sealed class CustomSimplifier : BaseSimplifier
    {
        public IDvExpr From { get; }
        public IDvExpr To { get; }

        public CustomSimplifier(IDvExpr from, IDvExpr to)
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
            Func<IDvExpr, IEnumerable<IDvExpr>> lst = el => new List<IDvExpr>() { el };

            return From is CommutativeAssociativeOperator from && from.Sign == expr.Sign ?
                expr.ReplaceSubOperands(from.Operands, lst(To)) :
                expr.ReplaceSubOperands(lst(From), lst(To))     ;
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
    }
}