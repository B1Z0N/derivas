using System;
using System.Text;
using System.Collections.Generic;

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

        string Represent();
    }


    // TODO: 
    //  expressions to add:
    //    add, multiply, divide, 
    //    raise to the power of, logarithm, 
    //    sin, arcsin, cos, arccos, tg, ctg
    //    arctg, arcct, sh, ch, th, cth
}
