using System;
using System.Collections.Generic;

using Derivas.Utils;

/// <summary>
/// Main userspace interface
/// (everything containing `Dv` is exposed to the user)
/// </summary>
namespace Derivas.Expression
{
    /// <summary>Userspace mathematical expression interface</summary>
    public interface IDvExpr
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        double Calculate(NameVal concrete);

        /// <summary>User readable representation of this expression</summary>
        string Represent();
    }

    internal abstract class Expr : IDvExpr, IEquatable<Expr>
    {
        public abstract double Calculate(NameVal concrete);
        public abstract bool Equals(Expr other);
        public abstract string Represent();
    }
}
