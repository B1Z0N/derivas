using System;

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

        /// <summary>Get the copy of yourself</summary>
        IDvExpr Clone();
    }

}