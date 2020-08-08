using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Simplifier
{
    internal abstract class BaseSimplifier : IDvSimplifier
    {
        public IDvExpr Simplify(IDvExpr expr)
            => expr switch
            {
                Logarithm log => Get(log),
                CommutativeAssociativeOperator op => Get(op),
                OrderedOperator op => Get(op),
                Operator op => Get(op),
                _ => Get(expr)
            };

        protected virtual IDvExpr Get(IDvExpr expr) => expr;
        protected virtual IDvExpr Get(Operator expr)
            => expr.CreateInstance(expr.Operands.Select(Simplify).ToArray());
        protected virtual IDvExpr Get(OrderedOperator expr) => expr;
        protected virtual IDvExpr Get(CommutativeAssociativeOperator expr) => expr;
        protected virtual IDvExpr Get(Logarithm log) => log;
    }

    internal sealed class ConstSimplifier : BaseSimplifier
    {
        private ConstSimplifier() { }
        public static ConstSimplifier Singleton { get; } = new ConstSimplifier();

        protected override IDvExpr Get(OrderedOperator expr)
        {
            var op = expr.CreateInstance(expr.Operands.Select(Simplify).ToArray());

            if (op.Operands.All(el => el is Constant))
            {
                return DvOps.Const(op.Calculate(new DvNameVal()));
            }
            else
            {
                return op;
            }
        }

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            var constsOnly = new List<double>();
            var newOps = new List<IDvExpr>();

            foreach (var op in expr.Operands)
            {
                var newOp = Simplify(op);

                if (newOp is Constant con)
                {
                    constsOnly.Add(con.Val);
                }
                else
                {
                    newOps.Add(newOp);
                }
            }

            IDvExpr res = new Constant(expr.OpFunc(constsOnly.ToArray()));
            newOps.Insert(0, res);
            return newOps.Count == 1 ? res : expr.CreateInstance(newOps.ToArray());
        }

        protected override IDvExpr Get(Logarithm log)
        {
            var res = Get(log as OrderedOperator);
            if (res is Constant)
            {
                return res;
            }
            
            log = res as Logarithm;
            if (log.Base is Constant bas && bas.Val == 1d)
            {
                return DvOps.Const(0);
            }

            return log;
        }
    }

    internal sealed class PolynomSimplifier : BaseSimplifier
    {
        private PolynomSimplifier() { }
        public static PolynomSimplifier Singleton { get; } = new PolynomSimplifier();

        protected override IDvExpr Get(OrderedOperator expr)
        {
            return base.Get(expr);
        }

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            return null;
        }

    }

    public sealed partial class DvSimplifier
    {
        public DvSimplifier ByConst()
        {
            InvocationQ.Enqueue(ConstSimplifier.Singleton);
            return this;
        }

        public DvSimplifier ByPolynom()
        {
            InvocationQ.Enqueue(PolynomSimplifier.Singleton);
            return this;
        }
    }
}
