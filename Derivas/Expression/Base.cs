using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

    /// <summary>
    /// Extends functionality of IDvExpr with cloning/creating capabilities
    /// </summary>
    internal abstract class CloneableExpr : IDvExpr
    {
        /// <summary>Deep copy if no args and instance with new params else</summary>
        protected abstract CloneableExpr CreateFromClonable(params CloneableExpr[] expr);

        /// <summary>Ensure that all IDvExpr is IClonable</summary>
        public CloneableExpr CreateInstance(params IDvExpr[] exprs)
            => CreateFromClonable(exprs.Select(
                expr => expr is CloneableExpr clexpr ? clexpr : new Wrapper(expr)
            ).ToArray());

        #region IDvExpr abstract implementation

        public abstract double Calculate(IDictionary<string, double> concrete);

        public abstract string Represent();

        public abstract bool Equals(IDvExpr other);

        #endregion IDvExpr abstract implementation

        /// <summary>
        /// Class to wrap all "uselses" userspace IDvExpr in IClonableExpr
        /// </summary>
        public class Wrapper : CloneableExpr
        {
            public IDvExpr Inner { get; }

            public Wrapper(IDvExpr inner) => Inner = inner;

            public override double Calculate(IDictionary<string, double> concrete) => Inner.Calculate(concrete);

            public override string Represent() => Inner.Represent();

            public override bool Equals(IDvExpr other) => Inner.Equals(other);

            public override int GetHashCode() => Inner.GetHashCode();

            protected override CloneableExpr CreateFromClonable(params CloneableExpr[] expr)
                => new Wrapper(Inner);
        }
    }

    public static partial class DvOps
    {
        /// <summary>
        /// Shortcut handlers, transforms numeric -> Const, string -> Symbol, IDvExpr -> IDvExpr
        /// and throws on others
        /// </summary>
        private static CloneableExpr CheckExpr(object arg) => Utils.CheckExpr(arg);

        /// <summary>Same as single arg function</summary>
        private static CloneableExpr[] CheckExpr(object[] args) => Utils.CheckExpr(args);
    }
}