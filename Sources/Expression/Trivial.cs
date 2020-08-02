using Derivas.Exception;
using Derivas.Utils;

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

        public Constant(double val) => Val = val;

        public override double Calculate(NameVal concrete) => Val;

        public override string Represent() => $"{Val}";

        public override bool Equals(object obj) => Equals(obj as Expr);

        public override bool Equals(Expr other)
            => (other as Constant)?.Val == Val;

        public override int GetHashCode() => Val.GetHashCode();
    }

    internal class Symbol : Expr
    {
        public string Name { get; }

        public Symbol(string name) => Name = name;

        public override double Calculate(NameVal concrete)
            => concrete.ContainsKey(Name) ? concrete[Name]
            : throw new DvSymbolMismatchException(Name);

        public override string Represent() => Name;

        public override bool Equals(object obj) => Equals(obj as Expr);

        public override bool Equals(Expr other)
            => (other as Symbol)?.Name == Name;

        public override int GetHashCode() => Name.GetHashCode();
    }
}