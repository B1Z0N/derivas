using Derivas.Expression;
using System;
using System.Linq;
using System.Collections.Generic;

using Derivas.Exception;

namespace Derivas.Expression
{
    // Some common mathemtaical constants

    public static partial class DvOps
    {
        public static IDvExpr E { get; } = new Constant(Math.E);
        public static IDvExpr PI { get; } = new Constant(Math.PI);
    }

    internal static class Utils
    {
        /// <summary>
        /// Check if expr is convertable to IDvExpr and perform conversions.
        /// It must be either numeric(int, double, ...) - for Const, string - for Sym or IDvExpr itself
        /// </summary>
        /// <returns>Converted IDvExpr or an exception if such conversion can't be done</returns>
        public static IDvExpr CheckExpr(object expr) =>
            IsNumber(expr) ?
            new Constant(Convert.ToDouble(expr)) :
            expr switch
            {
                string name => new Symbol(name),
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

        public static IDvExpr[] CheckExpr(object[] args) => args.Select(CheckExpr).ToArray();
    }
}