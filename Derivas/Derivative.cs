using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Expression;
using Derivas.Exception;

namespace Derivas.Derivative
{
    public class DvDerivativeMismatch : DvBaseException
    {
        public DvDerivativeMismatch(Type t) 
            : base($"There is no DvDerivative handler for type {t}")
        {
        }
    }

    public class DvDerivative<TNum>
    {
        public static IDvExpr<TNum> Get<TNum>(IDvExpr<TNum> expr) 
            => expr switch
            {
                DvSymbol<TNum> sym => Get(sym),
                DvConstant<TNum> con => Get(con),
                _ => throw new DvDerivativeMismatch(expr.GetType())
            };

        protected static DvConstant<TNum> Get(DvConstant<TNum> expr) => new DvConstant<TNum>(default(TNum));
        protected static DvConstant<TNum> Get(DvSymbol<TNum> expr) => new DvConstant<TNum> (default(TNum));
    }
}
