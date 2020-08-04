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
            => expr switch
            {
                MultiArgOperator op => Get(op),
                _ => Get(expr)
            };

        private IDvExpr Get(IDvExpr expr) => expr;
        private IDvExpr Get(MultiArgOperator expr)
        {
            var newOps = new List<IDvExpr>();
            var constsOnly = new List<double>();
            foreach (var op in expr.Operands)
            {
                if (op is Constant con)
                {
                    constsOnly.Add(con.Val);
                }
                else
                {
                    newOps.Add(op);
                }
            }

            newOps.Add(
                new Constant(expr.OpFunc(constsOnly.ToArray()))
            );
            return expr.CreateInstance(newOps.ToArray());
        }
    }

    internal sealed class PolynomSimplifier : IDvSimplifier
    {
        private PolynomSimplifier() { }
        public static PolynomSimplifier Singleton { get; }

        public IDvExpr Simplify(IDvExpr expr)
            => expr switch
            {
                MultiArgOperator op => Get(op),
                _ => Get(expr)
            };

        private IDvExpr Get(IDvExpr expr) => expr;
        private IDvExpr Get(MultiArgOperator expr)
        {
            throw new NotImplementedException();
        }
    }
}
