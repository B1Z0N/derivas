using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Expression;
using Derivas.Exception;

namespace Derivas.Expression
{
    internal abstract class OrderedOperator : Operator
    {
        #region base class functionality

        public override IEnumerable<IDvExpr> Operands { get => Operands_; }
        
        protected internal List<IDvExpr> Operands_;

        public OrderedOperator(params IDvExpr[] lst) => Operands_ = new List<IDvExpr>(lst);

        #endregion base class functionality
    }
}