using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Simplifier
{
    internal sealed class ConstSimplifier : IDvSimplifier
    {
        private ConstSimplifier() { }
        public static ConstSimplifier Singleton { get; }

        public IDvExpr Simplify(IDvExpr expr)
        {
            return Get(expr);
        }

        private IDvExpr Get(IDvExpr expr) => expr.Clone();
    }

    internal sealed class PolynomSimplifier : IDvSimplifier
    {
        private PolynomSimplifier() { }
        public static PolynomSimplifier Singleton { get; }

        public IDvExpr Simplify(IDvExpr expr)
        {
            return Get(expr);
        }

        private IDvExpr Get(IDvExpr expr) => expr.Clone();
    }
}
