using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Simplifier;
using Derivas.Expression;

namespace Derivas.Simplifier
{
    internal class MetaSymbol : IDvExpr, IDisposable
    {
        private static IDictionary<string, string> nameCorespondence =
            new Dictionary<string, string>();

        public string Name { get; }

        public MetaSymbol(string name) => Name = name;

        public void Dispose() => nameCorespondence.Remove(this.Name);

        #region interface implementation

        public double Calculate(Dictionary<string, double> concrete)
            => throw new NotImplementedException("You can't calculate Metasymbol's value");

        public override bool Equals(object other) => Equals(other as IDvExpr);
        public bool Equals(IDvExpr other)
        {
            if (other == null) return false;
            if (!(other is Symbol sym)) return false;
            
            if (nameCorespondence.ContainsKey(Name))
            {
                if (nameCorespondence[Name] == sym.Name)
                {
                    return true;
                }
                else
                {
                    nameCorespondence.Clear();
                    return false;
                }
            }
            else
            {
                nameCorespondence[Name] = sym.Name;
                return true;
            }
        }

        public string Represent() => 
            throw new NotImplementedException("Metasymbols can't be represented");

        public override int GetHashCode() => HashCode.Combine(Name, GetType());

        #endregion interface implementation
    }
}

namespace Derivas.Expression
{
    public static partial class DvOps
    {
        public static IDvExpr MetaSym(string name) => new MetaSymbol(name);
    }
}
