using Derivas.Expression;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Simplifier
{
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

    internal sealed class ConstSimplifier : BaseSimplifier
    {
        private ConstSimplifier()
        {
        }

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

            newOps.Insert(0, new Constant(expr.OpFunc(constsOnly.ToArray())));
            return newOps.Count == 1 ? newOps[0] : expr.CreateInstance(newOps.ToArray());
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
        private PolynomSimplifier()
        {
        }

        public static PolynomSimplifier Singleton { get; } = new PolynomSimplifier();

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            var coeffs = new Dictionary<IDvExpr, double>();
            foreach (var operand in expr.Operands)
            {
                if (coeffs.ContainsKey(operand))
                {
                    coeffs[operand] += 1d;
                }
                else
                {
                    coeffs[operand] = 1d;
                }
            }

            var res = new List<IDvExpr>();
            foreach (var pair in coeffs.AsEnumerable())
            {
                res.Add(
                    pair.Value == 1d ? pair.Key :
                    DvOps.Mul(DvOps.Const(pair.Value), pair.Key)
                );
            }

            return expr.CreateInstance(res.ToArray());
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