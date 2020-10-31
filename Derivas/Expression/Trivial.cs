using Derivas.Exception;
using System;
using System.Collections.Generic;

namespace Derivas.Expression
{
    internal class Constant : CloneableExpr
    {
        public double Val { get; }

        public Constant(double val) => Val = val;

        public override double Calculate(IDictionary<string, double> concrete) => Val;

        public override string Represent() => $"{Val}";

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public override bool Equals(IDvExpr other)
            => (other as Constant)?.Val == Val;

        public override int GetHashCode() => HashCode.Combine(Val, GetType());

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] expr)
            => expr.Length == 0 ? new Constant(Val) : expr[0].CreateInstance();
    }

    internal class Symbol : CloneableExpr
    {
        public string Name { get; }

        public Symbol(string name) => Name = name;

        public override double Calculate(IDictionary<string, double> concrete)
            => concrete.ContainsKey(Name) ? concrete[Name]
            : throw new DvSymbolMismatchException(Name);

        public override string Represent() => Name;

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public override bool Equals(IDvExpr other)
            => other is Symbol sym ? sym.Name == Name : false;

        public override int GetHashCode() => HashCode.Combine(Name, GetType());

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] expr)
            => expr.Length == 0 ? new Symbol(Name) : expr[0].CreateInstance();
    }
}