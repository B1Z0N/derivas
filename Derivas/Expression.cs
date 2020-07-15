using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;

namespace Derivas.Expression
{
    public class DvSymbolMismatchException : DvBaseException
    {
        public DvSymbolMismatchException(string shouldbe) 
            : base($"Value of '{shouldbe}' Symbol not included in the dictionary")
        {
        }
    }

    public interface IDvExpr<TNum>
    {
        TNum Calculate(IDictionary<string, TNum> nameVal);
        void Simplify();
        string ToString();
    }

    public class DvConstant<TNum> : IDvExpr<TNum>
    {
        public TNum Val { get; }

        public DvConstant(TNum val)
        {
            Val = val;
        }

        TNum IDvExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal)
        {
            return Val;
        }

        // Shortcut for userspace code
        public TNum Calculate() => Val;

        void IDvExpr<TNum>.Simplify()
        {
        }

        string IDvExpr<TNum>.ToString() => Val.ToString();
    }

    public class DvSymbol<TNum> : IDvExpr<TNum>
    {
        public string Name { get; }

        public DvSymbol(string name)
        {
            Name = name;
        }

        TNum IDvExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal)
        {
            nameVal.TryGetValue(Name, out var val);
            if (val == null) throw new DvSymbolMismatchException(Name);

            return val;
        }

        // Shortcut for userspace code
        public TNum Calculate(TNum val) => val;

        void IDvExpr<TNum>.Simplify()
        {
        }

        string IDvExpr<TNum>.ToString()
        {
            return Name;
        }
    }
}
