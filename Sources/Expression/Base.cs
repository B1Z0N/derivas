using System;
using System.Text;
using System.Collections.Generic;

using Derivas.Exception;

namespace Derivas.Expression
{
    public class DvSymbolMismatchException : DvBaseException
    {
        public DvSymbolMismatchException(string shouldbe) 
            : base($"Value of '{shouldbe}' Symbol not included in the dictionary")
        {
        }
    }

    /// <summary>Mathematical expression interface</summary>
    /// <typeparam name="TNum">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public interface IDvExpr<TNum>
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        TNum Calculate(IDictionary<string, TNum> nameVal);

        string Represent();
    }


    public static class DvExpr<TNum>
    {
        public static IDvExpr<TNum> Const(TNum n) => new DvConstant<TNum>(n);
        public static IDvExpr<TNum> Sym(string name) => new DvSymbol<TNum>(name);
        public static IDvExpr<TNum> Add(IDvExpr<TNum> fst, IDvExpr<TNum> snd) => new DvAddition<TNum>(fst, snd);
        public static IDvExpr<TNum> Mul(IDvExpr<TNum> fst, IDvExpr<TNum> snd) => new DvMultiplication<TNum>(fst, snd);
        public static IDvExpr<TNum> Div(IDvExpr<TNum> fst, IDvExpr<TNum> snd) => new DvDivision<TNum>(fst, snd);
        public static IDvExpr<TNum> Sub(IDvExpr<TNum> fst, IDvExpr<TNum> snd) => new DvSubtraction<TNum>(fst, snd);
    }

    // TODO: 
    //  expressions to add:
    //    raise to the power of, logarithm, 
    //    sin, arcsin, cos, arccos, tg, ctg
    //    arctg, arcct, sh, ch, th, cth
}
