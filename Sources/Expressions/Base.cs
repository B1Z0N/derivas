using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Main userspace interface
/// (everything containing `Dv` is exposed to the user)
/// </summary>
namespace Derivas.Expressions
{
    /// <summary>Mathematical expression interface</summary>
    public interface IDvExpr
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        double Calculate(IDictionary<string, double> nameVal);

        /// <summary>User readable representation of this expression</summary>
        string Represent();
    }
}
