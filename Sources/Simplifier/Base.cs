using System;
using System.Collections.Generic;
using System.Linq;

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

    internal abstract class BaseSimplifier : IDvSimplifier
    {
        public IDvExpr Simplify(IDvExpr expr)
            => expr switch
            {
                Logarithm log => Get(log),
                BinaryOperator op => Get(op),
                CommutativeAssociativeOperator op => Get(op),
                OrderedOperator op => Get(op),
                Operator op => Get(op),
                _ => Get(expr)
            };

        protected virtual IDvExpr Get(IDvExpr expr) => expr;

        protected virtual IDvExpr Get(Operator expr)
            => expr.CreateInstance(expr.Operands.Select(Simplify).ToArray());

        protected virtual IDvExpr Get(OrderedOperator expr) => Get(expr as Operator);

        protected virtual IDvExpr Get(CommutativeAssociativeOperator expr) => Get(expr as Operator);

        protected virtual IDvExpr Get(BinaryOperator expr) => Get(expr as OrderedOperator);

        protected virtual IDvExpr Get(Logarithm log) => Get(log as BinaryOperator);
    }
}
