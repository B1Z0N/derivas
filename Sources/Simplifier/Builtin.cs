﻿using Derivas.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Simplifier
{
    internal sealed class ConstSimplifier : BaseSimplifier
    {
        private ConstSimplifier()
        {
        }

        private static readonly Lazy<ConstSimplifier> lazy =
            new Lazy<ConstSimplifier>(() => new ConstSimplifier());
        public static ConstSimplifier Singleton => lazy.Value;

        protected override IDvExpr Get(OrderedOperator expr)
        {
            var op = expr.CreateInstance(expr.Operands.Select(Simplify).ToArray());

            if (op.Operands.All(el => el is Constant))
            {
                return DvOps.Const(op.Calculate(new Dictionary<string, double>()));
            }
            else
            {
                return op;
            }
        }

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            var constsOnly = new List<double>();
            var newOps = new LinkedList<IDvExpr>();

            foreach (var op in expr.Operands)
            {
                var newOp = Simplify(op);

                if (newOp is Constant con)
                {
                    constsOnly.Add(con.Val);
                }
                else
                {
                    newOps.AddLast(newOp);
                }
            }

            newOps.AddFirst(new Constant(expr.OpFunc(constsOnly.ToArray())));
            return newOps.Count == 1 ? newOps.First() : expr.CreateInstance(newOps.ToArray());
        }

        protected override IDvExpr Get(Logarithm log)
        {
            var res = base.Get(log);
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

        private static readonly Lazy<PolynomSimplifier> lazy =
            new Lazy<PolynomSimplifier>(() => new PolynomSimplifier());
        public static PolynomSimplifier Singleton => lazy.Value;

        protected override IDvExpr Get(CommutativeAssociativeOperator expr)
        {
            expr = base.Get(expr) as CommutativeAssociativeOperator;

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

    internal sealed class PartialSimplifier : BaseSimplifier
    {
        private Dictionary<string, double> Dict { get; }

        public PartialSimplifier(Dictionary<string, double> dict) => Dict = dict;

        protected override IDvExpr Get(Operator expr)
            => expr.CreateInstance(
                expr.Operands.Select(el => el switch
                {
                    Symbol sym => Dict.TryGetValue(sym.Name, out var val) ? DvOps.Const(val) : sym,
                    _ => Simplify(el)
                }).ToArray()
            );
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

        public DvSimplifier ByPartial(Dictionary<string, double> dict)
        {
            InvocationQ.Enqueue(new PartialSimplifier(dict));
            return this;
        }

        // shortcut
        public DvSimplifier ByPartial(string sym, double val)
            => ByPartial(new Dictionary<string, double>() { { sym, val } });
    }
}