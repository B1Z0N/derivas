using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Expression;
using Derivas.Exception;
using Derivas.Constant;

namespace Derivas.Expression
{
    public class DvDerivativeMismatch : DvBaseException
    {
        public DvDerivativeMismatch(Type t)
            : base($"There is no DvDerivative handler for type {t}")
        {
        }
    }
}

namespace Derivas.Derivative
{
    internal class DvDerivative<TNum> : IDvExpr<TNum>
    {
        private IDvExpr<TNum> DerivedExpr { get; }
        public IDvConstantsProvider<TNum> ConstantsProvider { get; set; }

        public DvDerivative(IDvExpr<TNum> expr, IDvConstantsProvider<TNum> constantsProvider = null)
        {
            ConstantsProvider = constantsProvider ?? new DvDefaultConstantsProvider<TNum>();
            DerivedExpr = Get(expr);
        }

        # region interface implementation

        TNum IDvExpr<TNum>.Calculate(IDictionary<string, TNum> nameVal) => DerivedExpr.Calculate(nameVal);
        string IDvExpr<TNum>.Represent() => DerivedExpr.Represent();

        # endregion

        # region derivative calculation

        private IDvExpr<TNum> Get(IDvExpr<TNum> expr)
            => expr switch
            {
                DvSymbol<TNum> sym => Get(sym),
                DvConstant<TNum> con => Get(con),
                _ => throw new DvDerivativeMismatch(expr.GetType())
            };

        protected DvConstant<TNum> Get(DvConstant<TNum> expr) => ConstantsProvider.Zero;
        protected DvConstant<TNum> Get(DvSymbol<TNum> expr) => ConstantsProvider.One;

        # endregion
    }
}
