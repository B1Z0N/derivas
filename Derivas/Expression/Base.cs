﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Derivas.Exception;

[assembly: InternalsVisibleTo("DerivasTests")]

namespace Derivas.Exception
{
    /// <summary>
    /// Symbol value not supplied during calculation
    /// </summary>
    public class DvExpressionMismatchException : DvBaseException
    {
        public Type WrongType;
        public DvExpressionMismatchException(Type t)
            : base($"You can't pass in {t} type, use int, string or IDvExpr")
        {
            WrongType = t;
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
        double Calculate(IDictionary<string, double> concrete);

        /// <summary>User readable representation of this expression</summary>
        string Represent();
    }

    public static partial class DvOps
    {
        /// <summary>
        /// Shortcut handlers, transforms numeric -> Const, string -> Symbol, IDvExpr -> IDvExpr 
        /// and throws on others
        /// </summary>
        private static IDvExpr CheckExpr(object arg) => Utils.CheckExpr(arg);
        /// <summary>Same as single arg function</summary>
        private static IDvExpr[] CheckExpr(object[] args) => Utils.CheckExpr(args);
    }

}