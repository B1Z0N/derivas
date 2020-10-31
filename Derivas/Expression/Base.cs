using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DerivasTests")]

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

        /// <summary>Ensure that all IDvExpr is Clonable</summary>
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
        /// Class to wrap all "useless" userspace IDvExpr in CloneableExpr
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
}