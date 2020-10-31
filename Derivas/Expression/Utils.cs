using Derivas.Exception;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

        internal static Dictionary<string, (int argN, Func<IDvExpr[], IDvExpr>)> DvTypeMap =>
            new Dictionary<string, (int argN, Func<IDvExpr[], IDvExpr>)>
            {
                [DvOpSigns.add] = (-1, DvOps.Add),
                [DvOpSigns.mul] = (-1, DvOps.Mul),
                [DvOpSigns.sub] = (2, (args) => DvOps.Sub(args[0], args[1])),
                [DvOpSigns.div] = (2, (args) => DvOps.Div(args[0], args[1])),
                [DvOpSigns.pow] = (2, (args) => DvOps.Pow(args[0], args[1])),
                [DvOpSigns.cos] = (1, (args) => DvOps.Cos(args.First())),
                [DvOpSigns.sin] = (1, (args) => DvOps.Sin(args.First())),
                [DvOpSigns.tan] = (1, (args) => DvOps.Tan(args.First())),
                [DvOpSigns.cotan] = (1, (args) => DvOps.Cotan(args.First())),
                [DvOpSigns.acos] = (1, (args) => DvOps.Acos(args.First())),
                [DvOpSigns.asin] = (1, (args) => DvOps.Asin(args.First())),
                [DvOpSigns.atan] = (1, (args) => DvOps.Atan(args.First())),
                [DvOpSigns.acotan] = (1, (args) => DvOps.Acotan(args.First())),
                [DvOpSigns.cosh] = (1, (args) => DvOps.Cosh(args.First())),
                [DvOpSigns.sinh] = (1, (args) => DvOps.Sinh(args.First())),
                [DvOpSigns.tanh] = (1, (args) => DvOps.Tanh(args.First())),
                [DvOpSigns.cotanh] = (1, (args) => DvOps.Cotanh(args.First())),
            };


        /// <summary>Class with common naming constants</summary>
        internal static class DvOpSigns
        {
            public const string add = "+";
            public const string mul = "*";
            public const string div = "/";
            public const string sub = "-";
            public const string pow = "^";
            public const string log = "log";
            public const string cos = "cos";
            public const string sin = "sin";
            public const string tan = "tan";
            public const string cotan = "cotan";
            public const string acos = "acos";
            public const string asin = "asin";
            public const string atan = "atan";
            public const string acotan = "acotan";
            public const string cosh = "cosh";
            public const string sinh = "sinh";
            public const string tanh = "tanh";
            public const string cotanh = "cotanh";
        }
    }
}