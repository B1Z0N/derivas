using Derivas.Exception;
using System;
using System.Collections.Generic;

namespace Derivas.Exception
{
    /// <summary>
    /// Symbol value not supplied during calculation
    /// </summary>
    public class DvSymbolMismatchException : DvBaseException
    {
        public string ShouldBe;
        public DvSymbolMismatchException(string shouldBe)
            : base($"Value of '{shouldBe}' Symbol is not included in the dictionary")
            => ShouldBe = shouldBe;
    }
}

namespace Derivas.Expression
{
    internal class Constant : IDvExpr
    {
        public double Val { get; }

        public Constant(double val) => Val = val;

        public double Calculate(IDictionary<string, double> concrete) => Val;

        public string Represent() => $"{Val}";

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public bool Equals(IDvExpr other)
            => (other as Constant)?.Val == Val;

        public override int GetHashCode() => HashCode.Combine(Val, GetType());

        public IDvExpr Clone() => new Constant(Val);
    }

    internal class Symbol : IDvExpr
    {
        public string Name { get; }

        public Symbol(string name) => Name = name;

        public double Calculate(IDictionary<string, double> concrete)
            => concrete.ContainsKey(Name) ? concrete[Name]
            : throw new DvSymbolMismatchException(Name);

        public string Represent() => Name;

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public bool Equals(IDvExpr other)
            => (other as Symbol)?.Name == Name;

        public override int GetHashCode() => HashCode.Combine(Name, GetType());

        public IDvExpr Clone() => new Symbol(Name);
    }

    public static partial class DvOps
    {
        public static IDvExpr Const(double val) => new Constant(val);
        public static IDvExpr Sym(string name) => new Symbol(name);
    }
}