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
        private static IDvExpr CheckExpr(object arg) => Utils.CheckExpr(arg);
        private static IDvExpr[] CheckExpr(object[] args) => Utils.CheckExpr(args);
    }

}