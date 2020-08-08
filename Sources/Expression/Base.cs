using System;
using System.Linq;

using Derivas.Exception;

namespace Derivas.Exception
{
    public class DvExpressionMismatch : DvBaseException
    {
        public DvExpressionMismatch(Type t)
            : base($"You can't pass in {t} type, use int, string or IDvExpr")
        {
        }
    }
}

/// <summary>
/// Main userspace interface
/// (everything containing `Dv` is exposed to the user)
/// </summary>
namespace Derivas.Expression
{
    /// <summary>Userspace mathematical expression interface</summary>
    public interface IDvExpr : IEquatable<IDvExpr>
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        double Calculate(DvNameVal concrete);

        /// <summary>User readable representation of this expression</summary>
        string Represent();
    }


    public static partial class DvOps
    {
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

        private static IDvExpr[] CheckExpr(object[] args) => args.Select(CheckExpr).ToArray();
    }

}