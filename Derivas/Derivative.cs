using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Expression;
using Derivas.Exception;
using Derivas.Constant;

namespace Derivas.Derivative
{
    public class DvDerivativeMismatch : DvBaseException
    {
        public DvDerivativeMismatch(Type t) 
            : base($"There is no DvDerivative handler for type {t}")
        {
        }
    }

    public class DvDerivative<TNum> : IDvExpr<TNum>
    {
        private IDvExpr<TNum> DerivedExpr { get; }
        public IDvConstantsProvider<TNum> ConstantsProvider { get; set; }

        public DvDerivative(IDvExpr<TNum> expr, IDvConstantsProvider<TNum> constantsProvider = null)
        {
            ConstantsProvider = constantsProvider ?? new DvDefaultConstantsProvider<TNum>();
            DerivedExpr = expr switch
            {
                DvSymbol<TNum> sym => Get(sym),
                DvConstant<TNum> con => Get(con),
                _ => throw new DvDerivativeMismatch(expr.GetType())
            };
        }

        TNum IDvExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal) => DerivedExpr.Calculate(nameVal);
        void IDvExpr<TNum>.Simplify() => DerivedExpr.Simplify();
        string IDvExpr<TNum>.ToString() => DerivedExpr.ToString();

        protected DvConstant<TNum> Get(DvConstant<TNum> expr) => ConstantsProvider.Zero;
        protected DvConstant<TNum> Get(DvSymbol<TNum> expr) => ConstantsProvider.One;
    }
}
