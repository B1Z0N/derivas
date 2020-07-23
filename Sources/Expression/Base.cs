using System;
using System.Text;
using System.Collections.Generic;

using Derivas.Exception;

namespace Derivas.Exception
{
    public class DvSymbolMismatchException : DvBaseException
    {
        public DvSymbolMismatchException(string shouldbe)
            : base($"Value of '{shouldbe}' Symbol not included in the dictionary")
        {
        }
    }

    public class DvExpressionMismatch : DvBaseException
    {
        public DvExpressionMismatch(Type t)
            : base($"You can't pass in {t} type, use int, string or IDvExpr")
        {
        }
    }
}

namespace Derivas.Expression
{
    /// <summary>Mathematical expression interface</summary>
    /// <typeparam name="TNum">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public interface IDvExpr<TNum>
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        TNum Calculate(IDictionary<string, TNum> nameVal);

        string Represent();
    }

    /// <summary>
    /// Laconic public API class to access different custom expressions
    /// You should always pass in int, string or an IDvExpr to this methods.
    /// </summary>
    /// <typeparam name="TNum">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public static class DvExpr<TNum>
    {

        # region public API shortcut methods

        public static IDvExpr<TNum> Const(TNum n) => new DvConstant<TNum>(n);
        public static IDvExpr<TNum> Sym(string name) => new DvSymbol<TNum>(name);

        public static IDvExpr<TNum> Add(object fst, object snd) => new DvAddition<TNum>(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr<TNum> Mul(object fst, object snd) => new DvMultiplication<TNum>(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr<TNum> Div(object fst, object snd) => new DvDivision<TNum>(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr<TNum> Sub(object fst, object snd) => new DvSubtraction<TNum>(CheckExpr(fst), CheckExpr(snd));

        # endregion

        /// <summary>
        /// Check if expr is convertable to IDvExpr and perform conversions.
        /// It must be either int(for Const), string(for Sym) of IDvExpr itself
        /// </summary>
        /// <returns>Converted IDvExpr or an exception if such conversion can't be done</returns>
        private static IDvExpr<TNum> CheckExpr(object expr)
            => expr switch
            {
                TNum number => Const(number),
                string name => Sym(name),
                IDvExpr<TNum> expression => expression,
                _ => throw new DvExpressionMismatch(expr.GetType())
            };
    }

    // TODO: 
    //  expressions to add:
    //    raise to the power of, logarithm, 
    //    sin, arcsin, cos, arccos, tg, ctg
    //    arctg, arcct, sh, ch, th, cth
}
