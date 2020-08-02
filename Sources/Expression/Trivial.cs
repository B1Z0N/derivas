using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Utils;
using Derivas.Exception;

namespace Derivas.Exception
{
    public class DvSymbolMismatchException : DvBaseException
    {
        public DvSymbolMismatchException(string shouldbe)
            : base($"Value of '{shouldbe}' Symbol is not included in the dictionary")
        {
        }
    }
}

namespace Derivas.Expression
{
    internal class Constant : Expr
    {
        public double Val { get; }

        public override double Calculate(NameVal concrete) => Val;
        public override string Represent() => $"{Val}";

        public override bool Equals(object obj) => Equals(obj as Expr);
        public override bool Equals(Expr other) 
            => (other as Constant)?.Val == Val;
        public override int GetHashCode() => Val.GetHashCode();
        public static bool operator ==(Constant left, Constant right)
            => left.Equals(right);
        public static bool operator !=(Constant left, Constant right)
            => !(left == right);
    }

    internal class Symbol : Expr
    {
        public string Name { get; }

        public override double Calculate(NameVal concrete) 
            => concrete.ContainsKey(Name) ? concrete[Name] 
            : throw new DvSymbolMismatchException(Name);
        public override string Represent() => Name;

        public override bool Equals(object obj) => Equals(obj as Expr);
        public override bool Equals(Expr other)
            => (other as Symbol)?.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();
        public static bool operator ==(Symbol left, Symbol right)
            => left.Equals(right);
        public static bool operator !=(Symbol left, Symbol right)
            => !(left == right);
    }
}
