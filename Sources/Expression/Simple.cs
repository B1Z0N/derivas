using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;

namespace Derivas.Expression
{
   internal class DvConstant<TNum> : IDvExpr<TNum>
    {
        public TNum Val { get; }

        public DvConstant(TNum val)
        {
            Val = val;
        }

        public TNum Calculate(IDictionary<string, TNum> nameVal) => Val;
        public string Represent() => Val.ToString();

        ///<summary>Shortcut for userspace code</summary>
        public TNum Calculate() => Val;
    }

    internal class DvSymbol<TNum> : IDvExpr<TNum>
    {
        public string Name { get; }

        public DvSymbol(string name)
        {
            Name = name;
        }

        public TNum Calculate(IDictionary<string, TNum> nameVal)
        {
            nameVal.TryGetValue(Name, out var val);
            if (val == null) throw new DvSymbolMismatchException(Name);

            return val;
        }

        public string Represent() => Name;

        ///<summary>Shortcut for userspace code</summary>
        public TNum Calculate(TNum val) => val;
    }
}
