

using Derivas.Exception;
using Derivas.Expression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Parser
{
    internal class Parser
    {
        private readonly List<Tokenizer.Token> PrefixExpr;

        public Parser(string infixExpr)
        {
            PrefixExpr = Tokenizer.ToPostfix(infixExpr).Reverse().ToList();
        }

        public IDvExpr GetExpr()
        {
            if (PrefixExpr.Count == 0) throw new System.Exception("");

            return GetExpr(0, out var _);
        }

        private IDvExpr GetExpr(int i, out int next)
        {
            if (i >= PrefixExpr.Count) throw new IndexOutOfRangeException("Out of arguments to parse");
            
            next = i + 1;
            var token = PrefixExpr[i];
            var trivial = TryCreateTrivial(token.Value);

            if (trivial != null)
            {
                return trivial;
            }
            else if (i == PrefixExpr.Count - 1) 
                throw new DvParserException($"Last input arg should be either symbol or constant, not: {token}");

            if (Utils.DvOpMap.TryGetValue(token.Value, out var info))
            {
                var (argN, creatF) = info;
                var args = new List<IDvExpr>();
                for (int j = 0; j < argN; ++j) args.Add(GetExpr(next, out next));

                return creatF(args.Reverse<IDvExpr>().ToArray());
            }
            else
            {
                // not trivial and not in operator types map
                throw new DvParserException($"Invalid token while parsing: '{token}'");
            }
        }

        private static IDvExpr TryCreateTrivial(string name)
        {
            if (Utils.DvOpMap.ContainsKey(name))
            {
                return null;
            }
            else
            {
                if (double.TryParse(name, out var constant))
                {
                    return DvOps.Const(constant);
                }
                else
                {
                    return DvOps.Sym(name);
                }
            }
        }

        public static IDvExpr Parse(string expr) => new Parser(expr).GetExpr();
    }
}