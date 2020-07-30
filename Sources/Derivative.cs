using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Expression;
using Derivas.Exception;
using static Derivas.Expression.DvExpr;

namespace Derivas.Expression
{
    public class DvDerivativeMismatch : DvBaseException
    {
        public DvDerivativeMismatch(Type t)
            : base($"There is no DvDerivative handler for type {t}")
        {
        }
    }
}

namespace Derivas.Derivative
{
    internal class DvDerivative : IDvExpr
    {
        private IDvExpr DerivedExpr { get; }
        private DvSymbol Symbol { get; }

        public DvDerivative(IDvExpr expr, string sym)
        {
            Symbol = new DvSymbol(sym);
            DerivedExpr = Get(expr);
        }

        # region interface implementation

        public double Calculate(IDictionary<string, double> nameVal) => DerivedExpr.Calculate(nameVal);
        public string Represent() => DerivedExpr.Represent();

        # endregion

        # region derivative calculation

        private IDvExpr Get(IDvExpr expr) => throw new DvDerivativeMismatch(expr.GetType());

        protected IDvExpr Get(DvConstant con) => Const(0);
        protected IDvExpr Get(DvSymbol sym) 
            => Const(sym.Equals(Symbol) ? 1 : 0);

        protected IDvExpr Get(DvAddition expr)
            => Add(expr.Operands.Select(el => Get(el)).ToArray());
        protected IDvExpr Get(DvMultiplication expr)
        {
            IDvExpr fst = expr.First, snd = expr.Second;
            return Add(Mul(Get(fst), snd), Mul(fst, Get(snd)));
        }
        protected IDvExpr Get(DvSubtraction expr)
        {
            IDvExpr fst = expr.First, snd = expr.Second;
            return Sub(Get(fst), Get(snd));
        }

        protected IDvExpr Get(DvDivision expr)
        {
            IDvExpr up = expr.First, low = expr.Second;
            return Div(
                Sub(
                    Mul(low, Get(up)),
                    Mul(Get(low), up)
                ), Pow(low, 2)
            );
        }

        // https://math.stackexchange.com/questions/1588166/derivative-of-functions-of-the-form-fxgx
        protected IDvExpr Get(DvExponantiation expr)
        {
            IDvExpr bas = expr.First, pow = expr.Second;
            return Mul(
                expr, Add(
                    Mul(Get(pow), Log(bas)), 
                    Div(Mul(pow, Get(bas)), bas)
                )
            );
        }

        protected IDvExpr Get(DvLogarithm expr)
        {
            IDvExpr bas = expr.Base, pow = expr.Of;
            return Mul(
                expr, Add(
                    Mul(Get(pow), Log(bas)),
                    Div(Mul(pow, Get(bas)), bas)
                )
            );
        }

        # endregion
    }
}
