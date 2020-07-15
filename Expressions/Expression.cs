using System;
using System.Collections.Generic;
using System.Text;

namespace Derivas.Expressions
{
    /// <summary>
    /// Base exception class for the project
    /// </summary>
    public class DerivasException : Exception
    {
        public DerivasException(string msg) : base(msg)
        { 
        }
    }

    public class SymbolMismatchException : DerivasException
    {
        public SymbolMismatchException(string shouldbe) 
            : base($"Value of '{shouldbe}' Symbol not included in the dictionary")
        {
        }
    }

    public interface IExpr<TNum>
    {
        TNum Calculate(IDictionary<string, TNum> nameVal);
        void Simplify();
        string ToString();
    }

    public class Symbol<TNum> : IExpr<TNum>
    {
        public string Name { get; }

        public Symbol(string name)
        {
            Name = name;
        }

        TNum IExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal)
        {
            nameVal.TryGetValue(Name, out var val);
            if (val == null) throw new SymbolMismatchException (Name);

            return val;
        }

        // Shortcut for userspace code
        public TNum Calculate(TNum val) => val;

        void IExpr<TNum>.Simplify()
        {
        }

        string IExpr<TNum>.ToString()
        {
            return Name;
        }
    }

    public class Constant<TNum> : IExpr<TNum>
    {
        public TNum Val { get; }

        public Constant(TNum val)
        {
            Val = val;
        }

        TNum IExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal)
        {
            return Val;
        }

        // Shortcut for userspace code
        public TNum Calculate() => Val;

        void IExpr<TNum>.Simplify()
        {
        }

        string IExpr<TNum>.ToString() => Val.ToString();
    }
}
