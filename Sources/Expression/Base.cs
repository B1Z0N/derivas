using System;
using System.Linq;
using System.Collections.Generic;



using Derivas.Exception;
using exp = System.Linq.Expressions.Expression;


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
            : base($"You can't pass in {t} type, use double, string or IDvExpr")
        {
        }
    }
}

namespace Derivas.Expression
{
    /// <summary>Mathematical expression interface</summary>
    /// <typeparam name="double">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public interface IDvExpr
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        double Calculate(IDictionary<string, double> nameVal);

        string Represent();
    }

    /// <summary>
    /// Laconic public API class to access different custom expressions
    /// You should always pass in int, string or an IDvExpr to this methods.
    /// </summary>
    /// <typeparam name="double">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public static class DvExpr
    {

        # region public API shortcut methods

        public static IDvExpr Const(double n) => new DvConstant(n);
        public static IDvExpr Sym(string name) => new DvSymbol(name);

        public static IDvExpr Add(params object[] args) => new DvAddition(args.Select(CheckExpr).ToArray());
        public static IDvExpr Mul(params object[] args) => new DvMultiplication(args.Select(CheckExpr).ToArray());
        public static IDvExpr Div(object fst, object snd) => new DvDivision(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr Sub(object fst, object snd) => new DvSubtraction(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr Pow(object fst, object snd) => new DvExponantiation(CheckExpr(fst), CheckExpr(snd));

        public static IDvExpr Log(object of, object _base = null)
            => new DvLogarithm(CheckExpr(of), _base == null ? null : CheckExpr(_base));

        public static IDvExpr Cos(object of) => new DvCosine(CheckExpr(of));
        public static IDvExpr Sin(object of) => new DvSine(CheckExpr(of));
        public static IDvExpr Tan(object of) => new DvTangens(CheckExpr(of));
        public static IDvExpr Cotan(object of) => new DvCotangens(CheckExpr(of));
        public static IDvExpr Acos(object of) => new DvArccosine(CheckExpr(of));
        public static IDvExpr Asin(object of) => new DvArcsine(CheckExpr(of));
        public static IDvExpr Atan(object of) => new DvArctangens(CheckExpr(of));
        public static IDvExpr Acotan(object of) => new DvArccotangens(CheckExpr(of));


        # endregion

        /// <summary>
        /// Check if expr is convertable to IDvExpr and perform conversions.
        /// It must be either numeric(int, double, ...) - for Const, string - for Sym or IDvExpr itself
        /// </summary>
        /// <returns>Converted IDvExpr or an exception if such conversion can't be done</returns>
        private static IDvExpr CheckExpr(object expr) =>
            IsNumber(expr) ?
            Const(Convert.ToDouble(expr)) :
            expr switch
            {
                string name => Sym(name),
                IDvExpr expression => expression,
                _ => throw new DvExpressionMismatch(expr.GetType())
            };

        private static bool IsNumber(object value)
        {
            return value is double
                    || value is int
                    || value is float
                    || value is decimal
                    || value is long
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is uint
                    || value is ulong
            ;
        }

        // TODO: 
        //  expressions to add:
        //    arctg, arcct, sh, ch, th, cth
    }

    /// <summary>Some common mathemtaical constants</summary>
    public readonly struct DvConsts
    {
        public static IDvExpr E { get; } = new DvConstant(Math.E);
        public static IDvExpr PI { get; } = new DvConstant(Math.PI);
    }
}