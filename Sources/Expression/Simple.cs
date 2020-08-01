using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;

namespace Derivas.Expression
{
    internal class DvConstant : IDvExpr, IEquatable<DvConstant>
    {
        public double Val { get; }

        public DvConstant(double val) => Val = val;

        public double Calculate(IDictionary<string, double> nameVal) => Val;
        public string Represent() => Val.ToString();

        #region equality and related stuff

        public override bool Equals(object obj) => Equals(obj as DvConstant);

        public bool Equals(DvConstant other)
            => other != null && Val == other.Val;

        public override int GetHashCode() => HashCode.Combine(Val);

        public static bool operator ==(DvConstant left, DvConstant right)
            => EqualityComparer<DvConstant>.Default.Equals(left, right);

        public static bool operator !=(DvConstant left, DvConstant right)
            => !(left == right);

        #endregion
    }

    internal class DvSymbol : IDvExpr, IEquatable<DvSymbol>
    {
        private string Name { get; }

        public DvSymbol(string name) => Name = name;

        public double Calculate(IDictionary<string, double> nameVal)
            => nameVal.ContainsKey(Name) ? nameVal[Name]
            : throw new DvSymbolMismatchException(Name);

        public string Represent() => Name;

        #region equality and related stuff

        public override bool Equals(object obj) => Equals(obj as DvSymbol);

        public bool Equals(DvSymbol other)
            => other != null && Name == other.Name;

        public override int GetHashCode() => HashCode.Combine(Name);

        public static bool operator ==(DvSymbol left, DvSymbol right)
            => EqualityComparer<DvSymbol>.Default.Equals(left, right);

        public static bool operator !=(DvSymbol left, DvSymbol right)
            => !(left == right);

        #endregion
    }
}
