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
    }

    internal class DvSymbol : IDvExpr
    {
        public string Name { get; }

        public DvSymbol(string name)
        {
            Name = name;
        }

        public double Calculate(IDictionary<string, double> nameVal) => 
            nameVal.ContainsKey(Name) ? 
            nameVal[Name] : 
            throw new DvSymbolMismatchException(Name);

        public string Represent() => Name;
    }
}
