using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Expression;
using Derivas.Exception;

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
    internal class DvDerivative : IDvExpr
    {
        private IDvExpr DerivedExpr { get; }

        public DvDerivative(IDvExpr expr)
        {
            DerivedExpr = Get(expr);
        }

        # region interface implementation

        double IDvExpr.Calculate(IDictionary<string, double> nameVal) => DerivedExpr.Calculate(nameVal);
        string IDvExpr.Represent() => DerivedExpr.Represent();

        # endregion

        # region derivative calculation

        private IDvExpr Get(IDvExpr expr)
            => expr switch
            {
                DvSymbol sym => Get(sym),
                DvConstant con => Get(con),
                _ => throw new DvDerivativeMismatch(expr.GetType())
            };

        protected DvConstant Get(DvConstant expr) => new DvConstant(0);
        protected DvConstant Get(DvSymbol expr) => new DvConstant(0);

        # endregion
    }
}
