using System;
using System.Text;
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

        public static IDvExpr Add(object fst, object snd) => new DvAddition(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr Mul(object fst, object snd) => new DvMultiplication(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr Div(object fst, object snd) => new DvDivision(CheckExpr(fst), CheckExpr(snd));
        public static IDvExpr Sub(object fst, object snd) => new DvSubtraction(CheckExpr(fst), CheckExpr(snd));

        # endregion

        /// <summary>
        /// Check if expr is convertable to IDvExpr and perform conversions.
        /// It must be either int(for Const), string(for Sym) of IDvExpr itself
        /// </summary>
        /// <returns>Converted IDvExpr or an exception if such conversion can't be done</returns>
        private static IDvExpr CheckExpr(object expr)
        {
            if (IsNumericType(expr))
            {
                var numericConstant = exp.Constant(expr, expr.GetType());
                var convertBody = exp.Convert(numericConstant, typeof(double));
                var lambda = exp.Lambda<Func<double>>(convertBody);
                double some = lambda.Compile().Invoke();
                return Const(some);
            }

            return expr switch
            {
                string name => Sym(name),
                IDvExpr expression => expression,
                _ => throw new DvExpressionMismatch(expr.GetType())
            };
        }

        private static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

    // TODO: 
    //  expressions to add:
    //    raise to the power of, logarithm, 
    //    sin, arcsin, cos, arccos, tg, ctg
    //    arctg, arcct, sh, ch, th, cth
}
