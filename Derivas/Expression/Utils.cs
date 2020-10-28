using Derivas.Exception;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Expression
{
    /// <summary>
    /// Shortcut class to handle dict creation
    /// </summary>
    public class DvDict
    {
        private Dictionary<string, double> Inner_ = new Dictionary<string, double>();

        public IDictionary<string, double> Get() => Inner_;

        public DvDict Add(string key, double val)
        {
            Inner_.Add(key, val);
            return this;
        }
    }

    internal static class Utils
    {
        /// <summary>
        /// Check if expr is convertable to IDvExpr and perform conversions.
        /// It must be either numeric(int, double, ...) - for Const, string - for Sym or IDvExpr itself
        /// </summary>
        /// <returns>Converted IDvExpr or an exception if such conversion can't be done</returns>
        public static CloneableExpr CheckExpr(object expr)
        {
            if (IsNumber(expr))
            {
                return new Constant(Convert.ToDouble(expr));
            }
            else
            {
                return expr switch
                {
                    string name => new Symbol(name),
                    CloneableExpr clexpression => clexpression,
                    IDvExpr expression => new CloneableExpr.Wrapper(expression),
                    _ => throw new DvExpressionMismatchException(expr.GetType())
                };
            }
        }

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

        public static CloneableExpr[] CheckExpr(object[] args) => args.Select(CheckExpr).ToArray();
    }
}