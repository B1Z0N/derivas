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

    /// <summary>Mathematical expression interface</summary>
    /// <typeparam name="TNum">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public interface IDvExpr<TNum>
    {
        /// <summary>Substitute symbols to it's values</summary>
        /// <param name="nameVal">Dictionary of symbolName:value</param>
        TNum Calculate(IDictionary<string, TNum> nameVal);

        /// <summary>Simplify an expression</summary>
        /// <example>"x + 5 + 8 + 3x" -> "4x + 13"</example>
        IDvExpr<TNum> Simplify();

        string ToString();
    }

    # region common expressions implementation

    public class DvConstant<TNum> : IDvExpr<TNum>
    {
        public TNum Val { get; }

        public DvConstant(TNum val)
        {
            Val = val;
        }

        TNum IDvExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal) => Val;
        IDvExpr<TNum> IDvExpr<TNum>.Simplify() => this;
        string IDvExpr<TNum>.ToString() => Val.ToString();

        ///<summary>Shortcut for userspace code</summary>
        public TNum Calculate() => Val;
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

        IDvExpr<TNum> IDvExpr<TNum>.Simplify() => this;
        string IDvExpr<TNum>.ToString() => Name;

        ///<summary>Shortcut for userspace code</summary>
        public TNum Calculate(TNum val) => val;
    }

    // TODO: 
    //  expressions to add:
    //    add, negate, multiply, divide, 
    //    raise to the power of, logarithm, 
    //    sin, arcsin, cos, arccos, tg, ctg
    //    arctg, arcct, sh, ch, th, cth

    # endregion
}
