using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Simplifier
{
    public interface IDvSimplifier
    {
        IDvExpr Simplify(IDvExpr expr);
    }

    public sealed partial class DvSimplifier
    {
        private IDvExpr Expr { get; set; }
        private Queue<IDvSimplifier> InvocationQ { get; } = new Queue<IDvSimplifier>();
        private DvSimplifier(IDvExpr expr) { Expr = expr; }

        public static DvSimplifier Create(IDvExpr expr)
            => new DvSimplifier(expr);

        public IDvExpr Simplify()
        {
            while (InvocationQ.Count != 0)
            {
                Expr = InvocationQ.Dequeue().Simplify(Expr);
            }

            return Expr;
        }
    }
}
