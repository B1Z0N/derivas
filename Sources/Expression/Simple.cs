using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;

namespace Derivas.Expression
{
   internal class DvConstant : IDvExpr
    {
        public double Val { get; }

        public DvConstant(double val)
        {
            Val = val;
        }

        public double Calculate(IDictionary<string, double> nameVal) => Val;
        public string Represent() => Val.ToString();

        ///<summary>Shortcut for userspace code</summary>
        public double Calculate() => Val;
    }

    internal class DvSymbol : IDvExpr
    {
        public string Name { get; }

        public DvSymbol(string name)
        {
            Name = name;
        }

        public double Calculate(IDictionary<string, double> nameVal)
        {
            nameVal.TryGetValue(Name, out var val);
            if (val == null) throw new DvSymbolMismatchException(Name);

            return val;
        }

        public string Represent() => Name;

        ///<summary>Shortcut for userspace code</summary>
        public double Calculate(double val) => val;
    }
}
